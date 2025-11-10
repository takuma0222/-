# 棚入庫・出庫機能仕様

## 概要
カード情報の工程が指定された値の場合、棚への入庫・出庫処理を行う機能です。
工程名や機能の有効/無効は設定ファイル（appsettings.json）で変更できます。

## 設定

### appsettings.json
```json
{
  "ShelfStorageLocation": "XXX",
  "EnableShelfRetrieval": true
}
```

- **ShelfStorageLocation**: 棚入庫対象となる工程（所在）の値（デフォルト: "XXX"）
- **EnableShelfRetrieval**: 棚出庫チェック機能を有効にするか（デフォルト: true）

### 設定例

#### 棚入庫工程を変更する場合
```json
{
  "ShelfStorageLocation": "棚保管",
  "EnableShelfRetrieval": true
}
```

#### 棚出庫チェックを無効にする場合
```json
{
  "ShelfStorageLocation": "XXX",
  "EnableShelfRetrieval": false
}
```

## 機能説明

### 1. 棚入庫処理
工程（所在列）が設定された値（デフォルト: "XXX"）のカードNoを入力した場合、自動的に棚入庫画面が表示されます。

#### 画面仕様
- **棚入庫画面**（モーダルダイアログ）
  - メッセージ：「入庫する棚番号を入れてください」
  - 入力欄：棚番号（1～10の整数）
  - ボタン：
    - **実行**：棚にカードを入庫して画面を閉じ、LAP厚選択へ進む
    - **キャンセル**：画面を閉じてカードNo入力前の状態に戻る

#### 処理フロー
1. カードNo入力（6桁）
2. カード情報取得
3. 所在が設定値（例: "XXX"）の場合、棚入庫画面を表示
4. 棚番号入力（1～10）
5. 実行ボタン押下
6. 棚ファイル（shelf_storage.csv）を更新
7. 画面クローズ
8. LAP厚選択へ進む

### 2. 棚出庫処理
すでに棚に入庫されているカードNoを入力した場合、自動的に棚出庫画面が表示されます。
※この機能は`EnableShelfRetrieval`設定で無効化できます。

#### 画面仕様
- **棚出庫画面**（モーダルダイアログ）
  - 表示情報：
    - 棚番号：入庫されている棚番号
    - カードNo：入庫されているカードNo
  - 入力欄：カードNo（読み取り専用）
  - ボタン：
    - **実行**：棚からカードを出庫して画面を閉じる
    - **キャンセル**：画面を閉じてカードNo入力前の状態に戻る

#### 処理フロー
1. カードNo入力（6桁）
2. 棚ファイル参照（EnableShelfRetrieval=trueの場合のみ）
3. カードNoが棚に入庫されている場合、棚出庫画面を表示
4. 実行ボタン押下
5. 棚ファイルから該当カードNoをクリア
6. 画面クローズ
7. メッセージ欄に「プロトス内のクッションを使ってください」と表示
8. カードNo入力前の状態に戻る（照合処理は行わない）

### 3. 棚ファイル仕様

#### ファイル名
`shelf_storage.csv`（アプリケーションと同じフォルダに自動生成）

#### ファイル形式
```csv
棚番号,カードNo
1,
2,
3,
4,
5,
6,
7,
8,
9,
10,
```

#### 初期状態
- 初回起動時に自動生成
- 棚1～10が事前作成される
- カードNo列は空

#### 入庫時
該当する棚番号の行のカードNo列に入庫したカードNoを記録
```csv
棚番号,カードNo
1,E00128
2,
3,
```

#### 出庫時
該当する棚番号の行のカードNo列をクリア
```csv
棚番号,カードNo
1,
2,
3,
```

## クラス構成

### Models/ShelfStorage.vb
棚保管情報のデータモデル
- プロパティ：
  - ShelfNo（棚番号）
  - CardNo（カードNo）

### Services/ShelfStorageManager.vb
棚保管情報の管理クラス
- メソッド：
  - Initialize()：初期化（CSVファイルがなければ作成）
  - FindShelfByCardNo(cardNo)：カードNoから棚番号を検索
  - StoreCard(shelfNo, cardNo)：棚にカードを入庫
  - RemoveCard(shelfNo)：棚からカードを出庫
  - IsShelfEmpty(shelfNo)：棚が空いているかチェック
  - GetAllStorages()：すべての棚情報を取得

### ShelfStorageForm.vb
棚入庫画面のフォーム
- プロパティ：
  - IsExecuted：実行ボタンが押下されたかのフラグ
  - SelectedShelfNo：選択された棚番号
- メソッド：
  - BtnExecute_Click：実行ボタンクリック処理
  - BtnCancel_Click：キャンセルボタンクリック処理

### ShelfRetrievalForm.vb
棚出庫画面のフォーム
- プロパティ：
  - IsExecuted：実行ボタンが押下されたかのフラグ
- メソッド：
  - BtnExecute_Click：実行ボタンクリック処理
  - BtnCancel_Click：キャンセルボタンクリック処理

## MainFormへの統合

### 追加フィールド
```vb
Private _shelfManager As ShelfStorageManager
```

### 初期化処理
```vb
Dim shelfCsvPath As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shelf_storage.csv")
_shelfManager = New ShelfStorageManager(shelfCsvPath)
```

### カードNo入力時処理（TxtCardNo_TextChanged）
```vb
' 工程が"XXX"の場合、棚入庫処理
If _currentCondition.Location = "XXX" Then
    HandleShelfStorage(cardNo)
    Return
End If

' 棚ファイルを参照してカードNoが入庫されているかチェック
Dim shelfStorage As ShelfStorage = _shelfManager.FindShelfByCardNo(cardNo)
If shelfStorage IsNot Nothing Then
    HandleShelfRetrieval(shelfStorage)
    Return
End If
```

### 棚入庫処理メソッド
```vb
Private Sub HandleShelfStorage(cardNo As String)
    Using storageForm As New ShelfStorageForm(cardNo, _shelfManager)
        Dim result As DialogResult = storageForm.ShowDialog(Me)
        
        If result = DialogResult.OK AndAlso storageForm.IsExecuted Then
            ' 入庫成功：LAP厚選択へ進む
            DisplayCardInfo(_currentCondition)
            cmbLapThickness.Enabled = True
            cmbLapThickness.Focus()
            ShowMessage("LAP厚を選択してください", Color.Black)
        Else
            ' キャンセル：カードNo入力前に戻る
            ResetToCardNoInput()
        End If
    End Using
End Sub
```

### 棚出庫処理メソッド
```vb
Private Sub HandleShelfRetrieval(shelfStorage As ShelfStorage)
    Using retrievalForm As New ShelfRetrievalForm(shelfStorage, _shelfManager)
        Dim result As DialogResult = retrievalForm.ShowDialog(Me)
        
        If result = DialogResult.OK AndAlso retrievalForm.IsExecuted Then
            ' 出庫成功：メッセージ表示してカードNo入力前に戻る
            ShowMessage("プロトス内のクッションを使ってください", Color.Blue)
            ResetToCardNoInput()
        Else
            ' キャンセル：カードNo入力前に戻る
            ResetToCardNoInput()
        End If
    End Using
End Sub
```

## テストデータ

card_conditions.csvに以下のテストデータを追加：
```csv
E00128,000006###,1,5,XXX棚入庫テスト,25,XXX
E00129,000007###,0,0,XXX棚入庫テスト2,50,XXX
```

## 使用例

### 入庫の例
1. カードNo「E00128」を入力
2. 棚入庫画面が表示される
3. 棚番号「3」を入力
4. 実行ボタンを押下
5. shelf_storage.csvが更新される（棚3にE00128が記録）
6. LAP厚選択へ進む

### 出庫の例
1. カードNo「E00128」を入力（すでに棚3に入庫されている）
2. 棚出庫画面が表示される（棚番号：3、カードNo：E00128）
3. 実行ボタンを押下
4. shelf_storage.csvが更新される（棚3のカードNoがクリア）
5. メッセージ「プロトス内のクッションを使ってください」が表示される
6. カードNo入力前の状態に戻る

## エラーハンドリング

### 入庫時のエラー
- 棚番号が1～10以外：エラーメッセージ表示
- 棚番号が空：エラーメッセージ表示
- すでにカードが入庫されている棚：エラーメッセージ表示

### 出庫時のエラー
- 棚が存在しない：エラーメッセージ表示

## 注意事項
- 棚ファイルは自動生成されるため、手動で作成する必要はありません
- 棚番号は1～10の範囲で固定です
- 同じ棚に複数のカードを入庫することはできません
- 出庫時は照合処理を行わず、カードNo入力前の状態に戻ります
