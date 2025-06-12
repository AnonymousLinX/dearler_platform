import { IShoppingCartInputDto } from "@/interfaces/IProductList";
import axios from "axios";

export const addCart = async(data: IShoppingCartInputDto) => {
    var res = await axios.post("ShoppingCart", data);
    return res;
}

export const getCart = async() => {
    var res = await axios.get("ShoppingCart");
    return res.data;
}

export const updateCartSelect = async(cartGuids: string[], cartSelected: boolean) =>{
    var res = await axios.post("ShoppingCart/CartSelected", {cartGuids, cartSelected});
    return res.data;
}