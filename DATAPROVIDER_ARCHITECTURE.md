# データプロバイダー アーキテクチャ

## 概要
従業員情報とカード情報の取得処理を将来的なソケット通信に対応しやすいように、インターフェースベースのアーキテクチャに変更しました。

## アーキテクチャ

### インターフェース

#### IEmployeeDataProvider
従業員情報を提供するインターフェース
- `Initialize()` - データプロバイダーを初期化
- `GetEmployeeName(employeeNo As String)` - 従業員番号から従業員名を取得

#### ICardDataProvider
カード情報を提供するインターフェース
- `Initialize()` - データプロバイダーを初期化
- `GetCardCondition(cardNo As String)` - カード番号からカード情報を取得

### 実装クラス

#### CSV実装（現在使用中）
- **EmployeeLoader** - CSVファイルから従業員情報を取得
- **CardConditionLoader** - CSVファイルからカード情報を取得

#### ソケット通信実装（将来実装用テンプレート）
- **SocketEmployeeDataProvider** - ソケット通信で従業員情報を取得
- **SocketCardDataProvider** - ソケット通信でカード情報を取得

### ファクトリークラス

**DataProviderFactory** - データソースの種類に応じて適切な実装を生成
```vb
Public Enum DataSourceType
    CSV          ' CSVファイルから取得
    Socket       ' ソケット通信で取得
End Enum
```

## 使用方法

### 現在（CSV使用）
```vb
' 従業員データプロバイダー
Dim employeeProvider As IEmployeeDataProvider = New EmployeeLoader(csvPath, logManager)
employeeProvider.Initialize()
Dim employeeName As String = employeeProvider.GetEmployeeName("123456")

' カードデータプロバイダー
Dim cardProvider As ICardDataProvider = New CardConditionLoader(csvPath)
cardProvider.Initialize()
Dim cardInfo As CardCondition = cardProvider.GetCardCondition("E00123")
```

### 将来（ソケット通信使用）
```vb
' ファクトリーを使用
Dim factory As New DataProviderFactory(DataProviderFactory.DataSourceType.Socket, logManager)

' 従業員データプロバイダー
Dim employeeProvider As IEmployeeDataProvider = factory.CreateEmployeeDataProvider(
    serverAddress:="192.168.1.100", 
    serverPort:=8080
)
employeeProvider.Initialize()
Dim employeeName As String = employeeProvider.GetEmployeeName("123456")

' カードデータプロバイダー
Dim cardProvider As ICardDataProvider = factory.CreateCardDataProvider(
    serverAddress:="192.168.1.100", 
    serverPort:=8081
)
cardProvider.Initialize()
Dim cardInfo As CardCondition = cardProvider.GetCardCondition("E00123")
```

## ソケット通信実装のガイドライン

### SocketEmployeeDataProvider / SocketCardDataProvider
これらのクラスは将来実装用のテンプレートとして作成されています。

実装時に必要な作業:
1. サーバーとの通信プロトコルを定義
2. `GetEmployeeName` / `GetCardCondition` メソッドにソケット通信ロジックを実装
3. レスポンスのパース処理を実装
4. エラーハンドリングを実装

### 実装例（コメント内に記載済み）
```vb
' リクエスト送信
Dim request As String = $"GET_EMPLOYEE:{employeeNo}"
Dim requestBytes As Byte() = Encoding.UTF8.GetBytes(request)
stream.Write(requestBytes, 0, requestBytes.Length)

' レスポンス受信
Dim buffer(1024) As Byte
Dim bytesRead As Integer = stream.Read(buffer, 0, buffer.Length)
Dim response As String = Encoding.UTF8.GetString(buffer, 0, bytesRead)
```

## 移行手順

1. ソケット通信メソッドを `SocketEmployeeDataProvider` / `SocketCardDataProvider` に実装
2. App.config に データソースタイプの設定を追加
3. MainForm で DataProviderFactory を使用するように変更
4. テスト環境で動作確認
5. 本番環境に展開

## 互換性

既存のコードとの互換性を保つため、以下のメソッドは残されています:
- `CardConditionLoader.LoadCondition(cardNo)` - 静的メソッド
- `CardConditionLoader.GetCondition(cardNo)` - インスタンスメソッド
- `EmployeeLoader.SearchAsync(employeeNo)` - 非同期メソッド

ただし、これらは非推奨となり、新しいインターフェースメソッドの使用を推奨します。
