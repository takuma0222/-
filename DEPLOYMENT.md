# デプロイメントガイド

## 配布パッケージの作成

### 必要なファイル
実行環境に配置する必要があるファイル：

```
BalanceInspection/
├── BalanceInspection.exe          # メインアプリケーション
├── BalanceInspection.exe.config   # アプリケーション設定
├── Newtonsoft.Json.dll            # JSON処理ライブラリ
├── appsettings.json              # 設定ファイル（自動生成可）
└── card_conditions.csv           # カード条件CSV（自動生成可）
```

### 配布パッケージの作成手順

1. **Releaseビルドを実行**
   ```bash
   msbuild BalanceInspection.sln /p:Configuration=Release
   ```

2. **必要なファイルを収集**
   ```bash
   mkdir deploy
   copy BalanceInspection\bin\Release\BalanceInspection.exe deploy\
   copy BalanceInspection\bin\Release\BalanceInspection.exe.config deploy\
   copy BalanceInspection\bin\Release\Newtonsoft.Json.dll deploy\
   ```

3. **ZIPファイルを作成**
   ```bash
   # PowerShell
   Compress-Archive -Path deploy\* -DestinationPath BalanceInspection_v1.0.0.zip
   ```

## インストール手順

### 前提条件
- Windows 7 SP1 以降、Windows 10/11
- .NET Framework 4.8（インストールされていない場合は自動的にインストールを促すメッセージが表示されます）

### インストール方法

1. **ZIPファイルを展開**
   配布されたZIPファイルを任意のフォルダに展開します。
   例: `C:\Program Files\BalanceInspection\`

2. **.NET Framework 4.8のインストール確認**
   アプリケーションを起動して、エラーが表示される場合は以下からインストール：
   https://dotnet.microsoft.com/download/dotnet-framework/net48

3. **設定ファイルの準備**
   
   初回起動時に自動生成されますが、事前に準備する場合：
   
   **appsettings.json の作成**
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
       },
       {
         "LogicalName": "Post_1mm",
         "PortName": "COM2",
         "BaudRate": 9600,
         "DataBits": 8,
         "Parity": "None",
         "StopBits": "One",
         "DeviceId": ""
       },
       {
         "LogicalName": "Post_5mm",
         "PortName": "COM3",
         "BaudRate": 9600,
         "DataBits": 8,
         "Parity": "None",
         "StopBits": "One",
         "DeviceId": ""
       }
     ]
   }
   ```

   **card_conditions.csv の作成**
   ```csv
   CardNo,投入前10mmクッション材,投入後1mmクッション材,投入後5mmクッション材,投入後10mmクッション材,エッジガード,気泡緩衝材
   e00123,1,2,0,0,1,5
   e00124,1,1,2,1,1,3
   e00125,2,1,1,0,1,10
   ```

4. **COMポートの設定**
   
   デバイスマネージャーで電子天秤が接続されているCOMポートを確認し、
   `appsettings.json` の `PortName` を適切に設定します。

5. **動作確認**
   
   `BalanceInspection.exe` を実行し、正常に起動することを確認します。

## 電子天秤の接続設定

### ハードウェア接続
1. 各EK-2000iをRS-232Cケーブル（D-sub 9ピン、ストレート）でPCに接続
2. デバイスマネージャーでCOMポート番号を確認
3. `appsettings.json` の各Balance設定を更新

### 天秤側の設定（EK-2000i）
以下のパラメータを設定（詳細は取扱説明書を参照）：

- **通信速度（bps）**: 9600（推奨）、または 1200/2400/4800
- **データビット**: 8
- **パリティ**: None（または Even/Odd）
- **ストップビット**: 1
- **モード**: コマンドモード（Command mode）推奨
- **終端**: CR+LF

### 動作モード

#### コマンドモード（推奨）
- アプリケーションから "Q" コマンドで計測値を取得
- 応答形式: `ST,GS,+0000.00g` （安定、グロス重量）

#### オートプリントモード（代替）
- 天秤が安定時に自動送信
- アプリケーションは受信のみ

## トラブルシューティング

### アプリケーションが起動しない
- .NET Framework 4.8がインストールされているか確認
- 管理者権限で実行してみる

### COMポートエラー
- デバイスマネージャーでポートが認識されているか確認
- 他のアプリケーションがポートを使用していないか確認
- ケーブルの接続を確認
- `appsettings.json` のPortNameが正しいか確認

### 通信タイムアウト
- 天秤の電源が入っているか確認
- ボーレート、パリティなどの設定が一致しているか確認
- `appsettings.json` の `ReadTimeoutMs` を増やす（例: 10000）

### カード条件が見つからない
- `card_conditions.csv` が正しい場所にあるか確認
- ファイルがUTF-8 BOMで保存されているか確認
- カード番号が正しく入力されているか確認

### ログが出力されない
- `logs` ディレクトリへの書き込み権限があるか確認
- ディスクに十分な空き容量があるか確認

## メンテナンス

### ログファイルの管理
- 検査ログ: `logs/yyyyMMdd.csv` (日次ローテーション)
- エラーログ: `logs/error/yyyy-MM-dd.log`
- 定期的に古いログファイルをアーカイブまたは削除

### カード条件の更新
1. アプリケーションを終了
2. `card_conditions.csv` を編集（UTF-8 BOMで保存）
3. アプリケーションを再起動

### 通信設定の変更
1. アプリケーションを終了
2. `appsettings.json` を編集
3. アプリケーションを再起動

## アンインストール

1. アプリケーションを終了
2. インストールフォルダを削除
3. 必要に応じてログファイルをバックアップ

## サポート

問題が解決しない場合は、以下の情報を添えてサポートに連絡してください：
- エラーメッセージのスクリーンショット
- `logs/error/` 内の最新のエラーログ
- 使用環境（OS、.NET Frameworkのバージョン）
- 天秤の型番と設定
