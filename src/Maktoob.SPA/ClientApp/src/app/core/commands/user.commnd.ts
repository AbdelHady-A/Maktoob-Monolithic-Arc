export interface SignUpUserCommand {
    UserName: string;
    Email: string;
    Password: string;
    FirstName: string;
    LastName: string;
}


export interface SignInUserCommand {
    Credentials: string;
    Password: string;
    EmailCredentials: boolean;
    RememberMe?: boolean;
    OperatingSystem?: string;
    Browser?: string;
    Device?: string;
    UserAgent?: string;
    OperatingSystemVersion?: string;
}


export interface SignOutUserCommand {
    UserId?: string;
    JwtId?: string;
    RefreshToken: string;
    LogOutAll?: boolean;
}

export interface RefreshTokenCommand {
    AccessToken: string;
    RefreshToken: string;
}