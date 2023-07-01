# SK-Demo-Template
Semantic Kernel を使って色々とテストするために最低限 Azure OpenAI に繋ぎに行く部分のサンプルコードです。

# How to use
1. まずはこのリポジトリをテンプレートにして新しいリポジトリを作る
2. ローカルにクローンして VSCode で開く
3. 以下のコマンドを打ってシークレットストアを作成
   1. dotnet user-secrets init
4. その後シークレットストアに Azure OpenAI に繋ぎに行くための情報を格納
   1. dotnet user-secrets set "DeployName" "デプロイ名"
   2. dotnet user-secrets set "Endpoint" "エンドポイント URL"
   3. dotnet user-secrets set "APIKey" "キー"
