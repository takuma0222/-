# Balance Inspection Desktop Application

電子天秤（A&D EK-2000iを3台）で計測した値を取得し、使用部材条件と照合して合否判定をするデスクトップアプリケーション。

## 概要

このアプリケーションは、3台の電子天秤（A&D EK-2000i）をRS-232C経由で接続し、計測値を取得して使用部材条件と照合し、合否判定を行います。

## 主な機能

- **従業員番号・カード番号の入力**: 6桁の英数字を入力
- **使用部材条件の表示**: CSVファイルからカード番号に対応する条件を読み込み
- **3台の天秤から計測値を取得**: RS-232C通信で順次取得
- **差分計算と照合**: 初回計測値と照合時計測値の差分を計算し、予定数と比較
- **合否判定**: 投入前10mm、投入後1mm、投入後5mmクッション材の3項目を照合
- **ログ出力**: 検査結果を日次ローテーションで CSV ファイルに記録
- **エラーハンドリング**: 通信エラーやタイムアウトを適切に処理

## プロジェクト構成

```
BalanceInspection/
├── MainForm.vb                    # メインフォーム（UI）
├── Models/
│   ├── AppConfig.vb              # アプリケーション設定モデル
│   ├── BalanceConfig.vb          # 天秤設定モデル
│   └── CardCondition.vb          # カード条件モデル
├── Services/
│   ├── ConfigLoader.vb           # 設定ファイル読み込み
│   ├── CardConditionLoader.vb    # カード条件CSV読み込み
│   ├── SerialPortManager.vb      # シリアルポート通信
│   ├── BalanceManager.vb         # 天秤管理（3台）
│   └── LogManager.vb             # ログ出力管理
└── My Project/                    # プロジェクト設定ファイル
```

## 技術仕様

- **言語**: VB.NET
- **フレームワーク**: .NET Framework 4.8
- **UI**: Windows Forms
- **通信**: RS-232C (System.IO.Ports.SerialPort)
- **設定ファイル**: JSON (Newtonsoft.Json)
- **データファイル**: CSV (UTF-8 BOM)

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
- `ReadTimeoutMs`: 通信タイムアウト（ミリ秒）
- `MaxRetries`: リトライ回数
- `Balances`: 天秤設定リスト（論理名、COMポート、ボーレートなど）

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

1. アプリケーションを起動
2. 従業員番号（6桁）を入力
3. カード番号（6桁）を入力
4. 使用部材条件が表示され、初回計測が自動実行される
5. 「照合」ボタンをクリックして照合時計測を実行
6. 合否判定結果が表示される
   - 合格: 「検査合格」と表示され、ログに記録
   - 不合格: 不一致項目が赤字で表示
7. 「キャンセル」ボタンで初期画面に戻る

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
