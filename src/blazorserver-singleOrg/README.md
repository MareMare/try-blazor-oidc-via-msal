# Blazor Serverアプリでの認証・認可

## テナントの作成

省略

## アプリの作成

Visual Studio 2022 でプロジェクトを新規作成すると AzureAD のアプリ登録までしてくれるようになってました。
* 新しいプロジェクトの追加

    ![](doc/blazorserver-singleOrg-00.png)

    ![](doc/blazorserver-singleOrg-01.png)

    ![](doc/blazorserver-singleOrg-03.png)

    ![](doc/blazorserver-singleOrg-04.png)

    ![](doc/blazorserver-singleOrg-05.png)

* 作成後

    ![](doc/blazorserver-singleOrg-06.png)

* nuget パッケージ参照

    以下の nuget パッケージ参照が追加されてます。
    * Microsoft.AspNetCore.Authentication.JwtBearer
    * Microsoft.AspNetCore.Authentication.OpenIdConnect
    * Microsoft.Identity.Web
    * Microsoft.Identity.Web.MicrosoftGraph
    * Microsoft.Identity.Web.UI

## アプリ登録内容の確認

Azure portal で [Azure Active Directory] に移動し登録内容を確認します。

* 概要

    ![](doc/blazorserver-singleOrg-07.png)

* 認証

    ![](doc/blazorserver-singleOrg-08.png)

* 証明書とシークレット

    ![](doc/blazorserver-singleOrg-09.png)

* APIのアクセス許可

    ![](doc/blazorserver-singleOrg-10.png)

* 初回のログイン時

    ![](doc/blazorserver-singleOrg-11.png)

## アプリの変更

### ログイン必須とするようにアプリを変更
既定でログイン必須となっているみたいなので変更不要です。

### ログインモードの変更
既定でリダイレクトの動作となっているので変更不要です。
