# Balance Inspection Desktop Application

電子天秤（A&D EK-2000iを3台）で計測した値を取得し、使用部材条件と照合して合否判定をするデスクトップアプリケーション。TCP通信システムとシミュレータを搭載。

## 概要

このアプリケーションは、3台の電子天秤（A&D EK-2000i）をTCP通信またはRS-232C経由で接続し、計測値を取得して使用部材条件と照合し、合否判定を行います。開発・テスト用の電子天秤シミュレータも含まれています。

## 主な機能

### 🎯 検品アプリケーション
- **従業員番号・カード番号の入力**: 6桁の英数字を入力
- **使用部材条件の表示**: CSVファイルからカード番号に対応する条件を読み込み
- **3台の天秤から計測値を取得**: TCP通信またはRS-232C通信で順次取得
- **自動初回計測**: カード番号入力完了で5秒タイムアウト付き自動計測
- **差分計算と照合**: 初回計測値と照合時計測値の差分を計算し、予定数と比較
- **合否判定**: 投入前10mm、投入後1mm、投入後5mmクッション材の3項目を照合
- **ログ出力**: 検査結果を日次ローテーションで CSV ファイルに記録
- **エラーハンドリング**: 通信エラーやタイムアウトを適切に処理

### 🧪 電子天秤シミュレータ
- **3台の天秤を同時シミュレート**: ポート9001-9003でTCPサーバー稼働
- **EK-iシリーズプロトコル対応**: 実機と同等のコマンド・レスポンス
- **重量値シミュレーション**: リアルタイムで重量値を生成・調整可能
- **接続状態表示**: 各天秤の接続状況をリアルタイム監視
- **テスト環境構築**: 実天秤なしでの開発・テストが可能

## プロジェクト構成

```
BalanceInspection/                 # 🎯 検品アプリケーション
├── MainForm.vb                    # メインフォーム（UI）
├── Models/
│   ├── AppConfig.vb              # アプリケーション設定モデル
│   ├── BalanceConfig.vb          # 天秤設定モデル（TCP/シリアル対応）
│   └── CardCondition.vb          # カード条件モデル
├── Services/
│   ├── ConfigLoader.vb           # 設定ファイル読み込み
│   ├── CardConditionLoader.vb    # カード条件CSV読み込み
│   ├── SerialPortManager.vb      # シリアルポート通信
│   ├── TcpCommunicationManager.vb # TCP通信マネージャー ✨ 新機能
│   ├── BalanceManager.vb         # 天秤管理（3台、TCP/シリアル統合）
│   └── LogManager.vb             # ログ出力管理
└── My Project/                    # プロジェクト設定ファイル

BalanceSimulator/                  # 🧪 電子天秤シミュレータ ✨ 新追加
└── BalanceSimulator/
    ├── Form1.vb                  # シミュレータメインUI
    ├── Program.vb                # アプリケーションエントリーポイント
    ├── SerialPortSimulator.vb    # 天秤シミュレーション機能
    └── BalanceSimulator.vbproj   # .NET 9.0プロジェクト
```

## 技術仕様

### 検品アプリケーション
- **言語**: VB.NET
- **フレームワーク**: .NET Framework 4.8
- **UI**: Windows Forms
- **通信**: TCP/IP (System.Net.Sockets) + RS-232C (System.IO.Ports.SerialPort)
- **設定ファイル**: JSON (Newtonsoft.Json)
- **データファイル**: CSV (UTF-8 BOM)

### 電子天秤シミュレータ
- **言語**: VB.NET
- **フレームワーク**: .NET 9.0
- **UI**: Windows Forms
- **通信**: TCP Server (System.Net.Sockets)
- **プロトコル**: EK-i series compatible
- **ポート**: 9001-9003 (Pre_10mm, Post_1mm, Post_5mm)

## ドキュメント

### クイックスタート
初めての方は [QUICKSTART.md](QUICKSTART.md) を参照してください。5分で環境をセットアップし、アプリケーションを試すことができます。

### 詳細ドキュメント
- **[BUILD.md](BUILD.md)**: ビルド手順と開発環境のセットアップ
- **[DEPLOYMENT.md](DEPLOYMENT.md)**: デプロイとインストール手順
- **[USER_MANUAL.md](USER_MANUAL.md)**: ユーザー向け操作マニュアル
- **[TECHNICAL_SPEC.md](TECHNICAL_SPEC.md)**: 技術仕様書と設計ドキュメント
- **[examples/README.md](examples/README.md)**: サンプル設定ファイルの説明

## 設定

### appsettings.json

- `LogDirectory`: ログ出力先ディレクトリ
- `CardConditionCsvPath`: カード条件CSVファイルパス  
- `ReadTimeoutMs`: 通信タイムアウト（ミリ秒、初回計測は5秒固定）
- `MaxRetries`: リトライ回数
- `Balances`: 天秤設定リスト（TCP/シリアル両対応）
  - `ConnectionType`: "TCP" または "Serial"
  - `TcpAddress`: TCP接続時のIPアドレス（デフォルト: 127.0.0.1）
  - `TcpPort`: TCP接続時のポート（9001-9003）
  - `PortName`: シリアル接続時のCOMポート

### card_conditions.csv

カード番号と使用部材条件の対応表。以下の列を含む：
- CardNo: カード番号（6桁英数字）
- 投入前10mmクッション材
- 投入後1mmクッション材
- 投入後5mmクッション材
- 投入後10mmクッション材
- エッジガード
- 気泡緩衝材

## 使用方法

### 🚀 クイックスタート（シミュレータ使用）

1. **シミュレータを起動**
   ```cmd
   BalanceSimulator\BalanceSimulator\bin\Debug\net9.0-windows\BalanceSimulator.exe
   ```

2. **検品アプリを起動**
   ```cmd
   BalanceInspection\bin\Debug\BalanceInspection.exe
   ```

3. **検品作業の実行**
   - 従業員番号（6桁）を入力
   - カード番号（6桁）を入力 → **自動で初回計測開始**
   - 使用部材条件が表示され、5秒タイムアウトで初回計測が完了
   - 「照合」ボタンをクリックして照合時計測を実行
   - 合否判定結果が表示される
     - 合格: 「検査合格」と表示され、ログに記録
     - 不合格: 不一致項目が赤字で表示
   - 「キャンセル」ボタンで初期画面に戻る

### 🔧 実運用環境での使用

実際の電子天秤を使用する場合は、`appsettings.json`で接続設定を変更：

```json
"ConnectionType": "Serial",
"PortName": "COM1",
"BaudRate": 9600
```

## ログ出力

### 検査ログ（logs/yyyyMMdd.csv）
- Timestamp: 日時
- EmployeeNo: 従業員番号
- CardNo: カード番号
- Pre10mm～BubbleInterference: 使用部材条件
- Result: 合否（OK/NG）

### エラーログ（logs/error/yyyy-MM-dd.log）
- 通信エラー、タイムアウト、その他の例外情報

## ライセンス

このプロジェクトは社内使用を目的としています。
