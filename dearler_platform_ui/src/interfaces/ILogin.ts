export  interface ILoginInfo{
    userNo: string;
    password: string;
    login: () => Promise<void>
}