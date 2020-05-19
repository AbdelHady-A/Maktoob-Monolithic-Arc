export class JwtClaimNames {
    public static readonly Actort = "actort";

    public static readonly Typ = "typ";

    public static readonly Sub = "sub";

    public static readonly Sid = "sid";

    public static readonly Prn = "prn";

    public static readonly Nbf = "nbf";

    public static readonly Nonce = "nonce";

    public static readonly NameId = "nameid";

    public static readonly Jti = "jti";

    public static readonly Iss = "iss";

    public static readonly Iat = "iat";

    public static readonly GivenName = "given_name";

    public static readonly FamilyName = "family_name";

    public static readonly Gender = "gender";

    public static readonly Exp = "exp";

    public static readonly Email = "email";

    public static readonly AtHash = "at_hash";

    public static readonly CHash = "c_hash";

    public static readonly Birthdate = "birthdate";

    public static readonly Azp = "azp";

    public static readonly AuthTime = "auth_time";

    public static readonly Aud = "aud";

    public static readonly Amr = "amr";

    public static readonly Acr = "acr";

    public static readonly UniqueName = "unique_name";

    public static readonly Website = "website";
}

export interface TokenModel {
    AccessToken: string;
    RefreshToken: string;
}

