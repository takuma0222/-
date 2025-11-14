# Balance Inspection Desktop Application

電子天秤（A&D EK-2000iを3台）で計測した値を取得し、使用部材条件と照合して合否判定をするデスクトップアプリケーション。TCP通信システムとシミュレータを搭載。

## 概要

このアプリケーションは、3台の電子天秤（A&D EK-2000i）をTCP通信またはRS-232C経由で接続し、計測値を取得して使用部材条件と照合し、合否判定を行います。開発・テスト用の電子天秤シミュレータも含まれています。

## 主な機能

### 🎯 検品アプリケーション
- **従業員番号チェック機能**: 6桁の従業員番号を入力すると自動でCSVから氏名を検索・表示（新機能✨）
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
│   ├── EmployeeLoader.vb         # 従業員情報CSV読み込み ✨ 新機能
│   ├── SerialPortManager.vb      # シリアルポート通信
│   ├── TcpCommunicationManager.vb # TCP通信マネージャー
│   ├── BalanceManager.vb         # 天秤管理（3台、TCP/シリアル統合）
│   └── LogManager.vb             # ログ出力管理
├── data/                          # データディレクトリ ✨ 新追加
│   └── employees.csv              # 従業員マスタCSV
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
- 品名: 製品名
- 枚数: ロット枚数
- 所在: 保管場所

## セットアップ（他端末での環境構築）

### 必要な環境
- Windows 10/11
- Visual Studio 2017以降（.NET Framework 4.7.1以降対応）
- または MSBuild 15.0以降

### クローンとビルド

1. **リポジトリをクローン**
   ```cmd
   git clone https://github.com/takuma0222/-.git
   cd -
   ```

2. **ビルド実行**
   ```cmd
   "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" BalanceInspection.sln /p:Configuration=Release
   ```
   
   または Visual Studio で `BalanceInspection.sln` を開いてビルド

3. **実行ファイルの場所**
   ```
   BalanceInspection\bin\Release\BalanceInspection.exe
   ```

4. **初回起動時の自動生成**
   - `appsettings.json`: アプリ設定（サンプルから自動生成）
   - `card_conditions.csv`: カード条件データ（サンプルから自動生成）
   - `logs/`: ログディレクトリ（自動作成）

### 注意事項
- NuGet パッケージは自動復元されます
- 設定ファイルのサンプルは `examples/` フォルダにあります
- 必要に応じて `appsettings.json` と `card_conditions.csv` を編集してください

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
   - **従業員番号（6桁）を入力** → 自動で氏名検索・表示 ✨ 新機能
     - 該当あり: 氏名が表示され、カード番号欄が有効化
     - 該当なし: 赤文字で「該当するユーザがありません。」と表示、入力欄クリア
   - カード番号（6桁）を入力 → **自動で初回計測開始**
   - 使用部材条件が表示され、5秒タイムアウトで初回計測が完了
   - 「照合」ボタンをクリックして照合時計測を実行
   - 合否判定結果が表示される
     - 合格: 「検査合格」と表示され、ログに記録
     - 不合格: 不一致項目が赤字で表示
   - 「キャンセル」ボタンで初期画面に戻る

#### 従業員マスタCSVの準備
`data/employees.csv` を以下の形式で準備してください：
```csv
123456,山田太郎
234567,佐藤花子
345678,鈴木一郎
```
- エンコーディング: UTF-8
- 区切り文字: カンマ
- 詳細は [EMPLOYEE_CHECK_GUIDE.md](BalanceInspection/EMPLOYEE_CHECK_GUIDE.md) を参照

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

MITライセンス

## 📊 業務フロー分析

### 業務システム化提案書
詳細な業務フロー分析と定量的効果測定については、以下のドキュメントをご参照ください：

- **[BUSINESS_FLOW_PROPOSAL.md](BUSINESS_FLOW_PROPOSAL.md)**: 包括的な業務フロー提案書
  - アプリケーション機能概要
  - 現行業務フロー（仮定）vs システム化後フロー
  - 定量的効果分析（72%時間削減、年間1,221万円削減）
  - 5つのMermaid業務フロー図
  - 導入推奨事項とROI分析

- **[BUSINESS_FLOW_SUMMARY.txt](BUSINESS_FLOW_SUMMARY.txt)**: クイックリファレンス要約

#### 主要メトリクス
- **作業時間**: 32分/回 → 9分/回 (72%削減)
- **年間削減**: 4,800時間 (2.4人分相当)
- **エラー率**: 20% → 0% (100%改善)
- **ROI**: 75倍、回収期間5日

