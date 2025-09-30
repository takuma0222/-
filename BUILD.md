# ビルド手順

## 必要な環境
- Visual Studio 2017以降（Visual Basic .NETサポート付き）
- .NET Framework 4.8

## NuGetパッケージのリストア
プロジェクトをビルドする前に、NuGetパッケージをリストアしてください：

```bash
nuget restore BalanceInspection.sln
```

または、Visual Studioで開いた際に自動的にリストアされます。

## ビルド方法

### Visual Studio でビルド
1. `BalanceInspection.sln` を Visual Studio で開く
2. メニューから「ビルド」→「ソリューションのビルド」を選択
3. `BalanceInspection\bin\Debug\` または `BalanceInspection\bin\Release\` に実行ファイルが生成されます

### コマンドラインでビルド
```bash
# Debugビルド
msbuild BalanceInspection.sln /p:Configuration=Debug

# Releaseビルド
msbuild BalanceInspection.sln /p:Configuration=Release
```

## 実行方法
ビルド後、以下のファイルを実行します：
```
BalanceInspection\bin\Debug\BalanceInspection.exe
```

## 設定ファイル
初回起動時、以下の設定ファイルが自動生成されます：

### appsettings.json
アプリケーション設定（COMポート、ボーレート、タイムアウトなど）

例：
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

### card_conditions.csv
カード番号と使用部材条件の対応表

例：
```csv
CardNo,投入前10mmクッション材,投入後1mmクッション材,投入後5mmクッション材,投入後10mmクッション材,エッジガード,気泡緩衝材
e00123,1,2,0,0,1,5
e00124,1,1,2,1,1,3
e00125,2,1,1,0,1,10
```

## トラブルシューティング

### COMポートが見つからない
- デバイスマネージャーで使用可能なCOMポートを確認
- `appsettings.json` の `PortName` を正しいポート名に変更

### Newtonsoft.Jsonが見つからない
```bash
nuget install Newtonsoft.Json -Version 13.0.3 -OutputDirectory packages
```

### ビルドエラー
- .NET Framework 4.8がインストールされているか確認
- Visual Studio のバージョンが2017以降であることを確認
