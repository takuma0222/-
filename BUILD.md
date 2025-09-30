# ビルド手順

## 必要な環境

### 検品アプリケーション
- Visual Studio 2017以降（Visual Basic .NETサポート付き）
- .NET Framework 4.8
- MSBuild (Visual Studio付属)

### 電子天秤シミュレータ
- .NET 9.0 SDK
- Visual Studio 2022以降（推奨）

## ビルド方法

### 🎯 検品アプリケーション

#### 1. NuGetパッケージのリストア
```bash
nuget restore BalanceInspection.sln
```

#### 2. Visual Studio でビルド
1. `BalanceInspection.sln` を Visual Studio で開く
2. メニューから「ビルド」→「ソリューションのビルド」を選択
3. `BalanceInspection\bin\Debug\` に実行ファイルが生成されます

#### 3. コマンドラインでビルド
```powershell
# MSBuildを使用（推奨）
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" "BalanceInspection\BalanceInspection.vbproj" /p:Configuration=Debug /p:Platform=AnyCPU

# または、ソリューション全体
msbuild BalanceInspection.sln /p:Configuration=Debug
```

### 🧪 電子天秤シミュレータ

#### 1. .NET 9.0 SDK のインストール
```bash
# Windows
winget install Microsoft.DotNet.SDK.9
```

#### 2. コマンドラインでビルド
```powershell
# シミュレータディレクトリに移動
cd BalanceSimulator\BalanceSimulator

# パッケージ復元とビルド
dotnet restore
dotnet build --configuration Debug

# Releaseビルド
dotnet build --configuration Release
```

#### 3. Visual Studio でビルド
1. `BalanceSimulator\BalanceSimulator\BalanceSimulator.vbproj` を開く
2. 「ビルド」→「プロジェクトのビルド」を選択
3. `bin\Debug\net9.0-windows\` に実行ファイルが生成されます

## 実行方法

### 🚀 統合テスト実行

#### 1. シミュレータを起動
```powershell
# 直接実行
BalanceSimulator\BalanceSimulator\bin\Debug\net9.0-windows\BalanceSimulator.exe

# またはdotnet run
cd BalanceSimulator\BalanceSimulator
dotnet run
```

#### 2. 検品アプリを起動
```powershell
BalanceInspection\bin\Debug\BalanceInspection.exe
```

#### 3. 接続確認
シミュレータが9001-9003ポートでリスニングしていることを確認：
```powershell
netstat -an | Select-String "900[1-3]"
```

### 🔧 実運用デプロイ
実際の電子天秤接続時は、`appsettings.json`で接続設定を変更してください。

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
      "ConnectionType": "TCP",
      "TcpAddress": "127.0.0.1",
      "TcpPort": 9001,
      "PortName": "SIM1",
      "BaudRate": 9600,
      "DataBits": 8,
      "Parity": "None",
      "StopBits": "One"
    },
    {
      "LogicalName": "Post_1mm", 
      "ConnectionType": "TCP",
      "TcpAddress": "127.0.0.1",
      "TcpPort": 9002,
      "PortName": "SIM2",
      "BaudRate": 9600,
      "DataBits": 8,
      "Parity": "None",
      "StopBits": "One"
    },
    {
      "LogicalName": "Post_5mm",
      "ConnectionType": "TCP", 
      "TcpAddress": "127.0.0.1",
      "TcpPort": 9003,
      "PortName": "SIM3",
      "BaudRate": 9600,
      "DataBits": 8,
      "Parity": "None",
      "StopBits": "One"
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
