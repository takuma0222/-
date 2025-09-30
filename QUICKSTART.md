# クイックスタートガイド

## 5分で始める検品デスクトップアプリ

### ステップ1: プロジェクトのビルド（開発者向け）

Visual Studioがインストールされている場合:

1. `BalanceInspection.sln` をVisual Studioで開く
2. メニューから「ビルド」→「ソリューションのビルド」
3. ビルドが完了したら、`BalanceInspection\bin\Debug\BalanceInspection.exe` が生成される

コマンドラインの場合:

```bash
# NuGetパッケージのリストア
nuget restore BalanceInspection.sln

# ビルド
msbuild BalanceInspection.sln /p:Configuration=Debug
```

### ステップ2: 設定ファイルの準備

初回起動時に自動生成されますが、事前に準備する場合:

```bash
# サンプルファイルをコピー
copy examples\appsettings.json BalanceInspection\bin\Debug\
copy examples\card_conditions.csv BalanceInspection\bin\Debug\
```

**重要**: 実際のCOMポート番号に合わせて `appsettings.json` を編集してください。

### ステップ3: 電子天秤の接続（実機がある場合）

1. 3台のEK-2000iをRS-232CケーブルでPCに接続
2. デバイスマネージャーでCOMポート番号を確認
3. `appsettings.json` の各Balance設定を更新:
   ```json
   "PortName": "COM1"  # 実際のポート番号に変更
   ```

### ステップ4: アプリケーションの起動

```bash
cd BalanceInspection\bin\Debug
BalanceInspection.exe
```

または、Windowsエクスプローラーから `BalanceInspection.exe` をダブルクリック

### ステップ5: 基本操作を試す

1. **従業員番号を入力**: `123456`（6桁の英数字）
2. **カード番号を入力**: `e00123`（6桁の英数字）
3. 使用部材条件が表示され、初回計測が自動実行される
4. **照合ボタンをクリック**: 照合時計測が実行され、合否判定が表示される
5. **キャンセルボタンをクリック**: 初期画面に戻る

## 実機なしでの動作確認（開発・テスト）

実際の電子天秤が接続されていない場合、アプリケーションは起動しますが計測時にエラーが表示されます。これは正常な動作です。

UI の動作確認のみ行う場合:
- 従業員番号とカード番号の入力検証
- 使用部材条件の表示（計測エラーは無視）
- キャンセル機能

## よくある質問

### Q: "ポート COM1 を開けませんでした" というエラーが出る
A: 正常です。実機が接続されていない場合は計測機能は使用できませんが、UIの確認は可能です。

### Q: "Newtonsoft.Jsonが見つかりません" というエラーが出る
A: NuGetパッケージをリストアしてください:
```bash
nuget restore BalanceInspection.sln
```

### Q: ビルドエラーが出る
A: 
- .NET Framework 4.8がインストールされているか確認
- Visual Studio 2017以降を使用しているか確認
- NuGetパッケージが正しくリストアされているか確認

### Q: 実機テストをしたい
A: 以下を準備してください:
- A&D EK-2000i 3台
- RS-232Cケーブル（D-sub 9ピン、ストレート）×3本
- USB-RS232C変換アダプタ（PCにシリアルポートがない場合）

## 次のステップ

詳細な情報は以下のドキュメントを参照してください:

- **ビルド方法**: [BUILD.md](BUILD.md)
- **デプロイ手順**: [DEPLOYMENT.md](DEPLOYMENT.md)
- **操作マニュアル**: [USER_MANUAL.md](USER_MANUAL.md)
- **技術仕様**: [TECHNICAL_SPEC.md](TECHNICAL_SPEC.md)

## サポート

問題が発生した場合:
1. [USER_MANUAL.md](USER_MANUAL.md) のトラブルシューティングセクションを確認
2. `logs/error/` ディレクトリのエラーログを確認
3. GitHubのIssueを作成して報告

## 開発に参加する

貢献を歓迎します！以下の手順で開発に参加できます:

1. リポジトリをフォーク
2. 機能ブランチを作成 (`git checkout -b feature/AmazingFeature`)
3. 変更をコミット (`git commit -m 'Add some AmazingFeature'`)
4. ブランチにプッシュ (`git push origin feature/AmazingFeature`)
5. プルリクエストを作成

---

**最終更新**: 2024年
**バージョン**: 1.0.0
