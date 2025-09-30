# 技術仕様書

## システム概要

### 目的
A&D EK-2000i電子天秤3台を使用した部材投入検査システムの構築

### スコープ
- 3台の電子天秤からRS-232C経由で計測値を取得
- カード番号に基づく使用部材条件の照合
- 投入前後の差分計算と予定数との比較
- 検査結果のログ記録

## アーキテクチャ

### レイヤー構造

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│         (MainForm.vb)                   │
│  - UI Components                        │
│  - User Input Validation                │
│  - Message Display                      │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│         Business Logic Layer            │
│  - BalanceManager.vb                    │
│  - Measurement Flow Control             │
│  - Validation Logic                     │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│         Service Layer                   │
│  - SerialPortManager.vb                 │
│  - ConfigLoader.vb                      │
│  - CardConditionLoader.vb               │
│  - LogManager.vb                        │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│         Data Layer                      │
│  - JSON Configuration Files             │
│  - CSV Data Files                       │
│  - Log Files                            │
└─────────────────────────────────────────┘
```

## クラス設計

### Models

#### AppConfig
アプリケーション設定を保持

```vb
Public Class AppConfig
    Public Property LogDirectory As String
    Public Property CardConditionCsvPath As String
    Public Property Balances As List(Of BalanceConfig)
    Public Property ReadTimeoutMs As Integer
    Public Property MaxRetries As Integer
End Class
```

#### BalanceConfig
天秤設定を保持

```vb
Public Class BalanceConfig
    Public Property LogicalName As String
    Public Property PortName As String
    Public Property BaudRate As Integer
    Public Property DataBits As Integer
    Public Property Parity As String
    Public Property StopBits As String
    Public Property DeviceId As String
End Class
```

#### CardCondition
カード条件を保持

```vb
Public Class CardCondition
    Public Property CardNo As String
    Public Property Pre10mm As Integer
    Public Property Post1mm As Integer
    Public Property Post5mm As Integer
    Public Property Post10mm As Integer
    Public Property EdgeGuard As Integer
    Public Property BubbleInterference As Integer
End Class
```

### Services

#### ConfigLoader
JSON設定ファイルの読み込み

**責務**:
- appsettings.jsonの読み込み
- デフォルト設定の生成
- 設定の検証

**主要メソッド**:
- `Load() As AppConfig`

#### CardConditionLoader
CSV条件ファイルの読み込み

**責務**:
- card_conditions.csvの読み込み
- カード番号による条件検索
- サンプルCSVの生成

**主要メソッド**:
- `GetCondition(cardNo As String) As CardCondition`
- `Reload()`

#### SerialPortManager
シリアルポート通信の管理

**責務**:
- COMポートの開閉
- 計測コマンドの送信
- 応答データの受信とパース
- タイムアウトとリトライ処理

**主要メソッド**:
- `Open()`
- `Close()`
- `ReadValue() As Double`

**通信プロトコル**:
- コマンド: "Q" + CR + LF
- 応答: "ST,GS,+0000.00g" + CR + LF
- タイムアウト: 設定可能（デフォルト5秒）
- リトライ: 設定可能（デフォルト3回）

#### BalanceManager
3台の天秤を統合管理

**責務**:
- 複数SerialPortManagerの統合管理
- 初回計測の実行
- 照合時計測の実行
- 差分計算

**主要メソッド**:
- `OpenAll()`
- `CloseAll()`
- `PerformInitialReading()`
- `PerformVerificationReading()`
- `CalculateDifferences() As Dictionary(Of String, Double)`

#### LogManager
ログ出力の管理

**責務**:
- 検査ログの出力
- エラーログの出力
- 日次ローテーション

**主要メソッド**:
- `WriteInspectionLog(...)`
- `WriteErrorLog(errorMessage As String)`

## 通信仕様

### RS-232C設定

| パラメータ | 設定値 | 備考 |
|-----------|--------|------|
| ボーレート | 1200/2400/4800/9600 | デフォルト: 9600 |
| データビット | 7/8 | デフォルト: 8 |
| パリティ | None/Even/Odd | デフォルト: None |
| ストップビット | 1/2 | デフォルト: 1 |
| フロー制御 | なし | - |
| ケーブル | D-sub 9ピン | ストレート |

### EK-iシリーズコマンド

#### 計測値要求コマンド
```
送信: Q<CR><LF>
応答: ST,GS,+0000.00g<CR><LF>
```

**応答フォーマット**:
- ST: 安定状態（ST=安定、US=不安定、OL=オーバーロード）
- GS: 表示重量タイプ（GS=グロス、NT=ネット）
- +0000.00g: 重量値（符号付き）+ 単位

#### エラー応答
```
EC,xx<CR><LF>
```
- xx: エラーコード

### データパース処理

1. 応答文字列を「,」で分割
2. 1番目の要素で状態を確認（ST以外はエラー）
3. 3番目の要素から重量値を抽出
4. 単位文字（g, kg等）を除去
5. 数値に変換

## データフロー

### 初回計測フロー

```
1. カードNo入力完了
2. CardConditionLoader.GetCondition()
3. 条件をDataGridViewに表示
4. BalanceManager.PerformInitialReading()
   4.1. SerialPortManager[0].ReadValue() → Pre_10mm
   4.2. SerialPortManager[1].ReadValue() → Post_1mm
   4.3. SerialPortManager[2].ReadValue() → Post_5mm
5. 計測値を内部保存（初回計測値）
6. "照合"ボタンを有効化
```

### 照合フロー

```
1. "照合"ボタンクリック
2. 従業員No検証（6桁）
3. BalanceManager.PerformVerificationReading()
   3.1. SerialPortManager[0].ReadValue() → Pre_10mm
   3.2. SerialPortManager[1].ReadValue() → Post_1mm
   3.3. SerialPortManager[2].ReadValue() → Post_5mm
4. 差分計算
   diff[Pre_10mm] = verification[Pre_10mm] - initial[Pre_10mm]
   diff[Post_1mm] = verification[Post_1mm] - initial[Post_1mm]
   diff[Post_5mm] = verification[Post_5mm] - initial[Post_5mm]
5. 照合判定
   diff[Pre_10mm] == condition.Pre10mm AND
   diff[Post_1mm] == condition.Post1mm AND
   diff[Post_5mm] == condition.Post5mm
6. 結果表示
   OK: "検査合格"（緑色）→ ログ記録 → "照合"ボタン無効化
   NG: "NG:項目名:差分≠予定"（赤色）→ ログ記録
```

## ファイルフォーマット

### appsettings.json

```json
{
  "LogDirectory": "logs",
  "CardConditionCsvPath": "card_conditions.csv",
  "ReadTimeoutMs": 5000,
  "MaxRetries": 3,
  "Balances": [
    {
      "LogicalName": "Pre_10mm",
      "PortName": "COM1",
      "BaudRate": 9600,
      "DataBits": 8,
      "Parity": "None",
      "StopBits": "One",
      "DeviceId": ""
    }
  ]
}
```

### card_conditions.csv

```csv
CardNo,投入前10mmクッション材,投入後1mmクッション材,投入後5mmクッション材,投入後10mmクッション材,エッジガード,気泡緩衝材
e00123,1,2,0,0,1,5
```

**制約**:
- エンコーディング: UTF-8 BOM
- 1行目: ヘッダー行（固定）
- 2行目以降: データ行
- CardNo: 一意
- 数値項目: 整数（0-99）

### 検査ログ（logs/yyyyMMdd.csv）

```csv
Timestamp,EmployeeNo,CardNo,Pre10mm,Post1mm,Post5mm,Post10mm,EdgeGuard,BubbleInterference,Result
2024-01-15 10:30:45,123456,e00123,1,2,0,0,1,5,OK
```

**フォーマット**:
- エンコーディング: UTF-8 BOM
- タイムスタンプ: yyyy-MM-dd HH:mm:ss
- Result: OK/NG

### エラーログ（logs/error/yyyy-MM-dd.log）

```
[2024-01-15 10:30:45.123] タイムアウト: COM1 (Pre_10mm)
[2024-01-15 10:31:02.456] 応答のパースに失敗: US,GS,+0001.23g
```

**フォーマット**:
- エンコーディング: UTF-8 BOM
- タイムスタンプ: yyyy-MM-dd HH:mm:ss.fff
- メッセージ: 自由形式

## エラーハンドリング

### 通信エラー

| エラー | 原因 | 対処 |
|--------|------|------|
| TimeoutException | 天秤からの応答なし | リトライ → 最大回数でエラー表示 |
| IOException | ポートオープン失敗 | エラーメッセージ表示 |
| UnauthorizedAccessException | ポート使用中 | エラーメッセージ表示 |

### データエラー

| エラー | 原因 | 対処 |
|--------|------|------|
| パース失敗 | 不正な応答形式 | エラーログ記録 → リトライ |
| 不安定状態（US） | 天秤が不安定 | リトライ |
| オーバーロード（OL） | 重量オーバー | エラー表示 |

### 設定ファイルエラー

| エラー | 原因 | 対処 |
|--------|------|------|
| FileNotFoundException | ファイルなし | デフォルト設定を生成 |
| JsonException | JSON形式エラー | エラーメッセージ表示 → 終了 |

## パフォーマンス

### 計測時間

- 1台あたりの計測時間: 約1-2秒
- 3台の順次計測: 約3-6秒
- タイムアウト発生時: 最大 ReadTimeoutMs × MaxRetries × 3台

### メモリ使用量

- アプリケーション起動時: 約20-30MB
- 通常動作時: 約30-40MB

## セキュリティ

### データ保護
- ログファイルへのアクセス制御はOS依存
- 通信データの暗号化なし（ローカル接続のため）

### 認証・認可
- 従業員番号の入力のみ（認証機能なし）
- システムレベルのアクセス制御に依存

## 拡張性

### 将来的な拡張ポイント

1. **天秤台数の変更**
   - `appsettings.json` の Balances 配列に追加/削除
   - UI の調整が必要

2. **通信方式の変更**
   - オートプリントモードへの切り替え
   - SerialPortManager のロジック変更

3. **認証機能の追加**
   - 従業員番号の照合
   - データベースとの連携

4. **バーコードリーダー対応**
   - カード番号入力の自動化
   - イベントハンドラーの追加

5. **ネットワーク対応**
   - 検査結果の自動送信
   - 中央データベースへの記録

## テスト要件

### 単体テスト
- SerialPortManager のモック化
- ConfigLoader のファイルI/Oテスト
- CardConditionLoader のパース処理テスト
- 差分計算ロジックのテスト

### 結合テスト
- 実機での通信テスト
- エラーハンドリングのテスト
- ログ出力のテスト

### システムテスト
- 正常系フローの動作確認
- 異常系（通信断、タイムアウト等）の動作確認
- 長時間稼働テスト

## 保守・運用

### ログ監視
- エラーログの定期確認
- ディスク使用量の監視

### 定期メンテナンス
- 古いログファイルの削除/アーカイブ（推奨: 月次）
- 設定ファイルのバックアップ（推奨: 週次）

### トラブル時の対応
1. エラーログの確認
2. 通信設定の確認
3. 天秤の接続確認
4. アプリケーションの再起動
5. PCの再起動
