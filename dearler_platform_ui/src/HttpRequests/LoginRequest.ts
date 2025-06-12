import { ILoginInfo } from "@/interfaces/ILogin";
import axios from "axios";

export const userLogin = async (data: object) => {
    var res = await axios.post("Login", data);
    return res;
}
