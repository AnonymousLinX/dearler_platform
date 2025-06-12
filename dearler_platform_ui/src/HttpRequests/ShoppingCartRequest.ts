import { IShoppingCartInputDto } from "@/interfaces/IProductList";
import { ShoppingCartSelectedEditDTO } from "@/interfaces/IShoppingCart";
import axios from "axios";

export const addCart = async(data: IShoppingCartInputDto) => {
    var res = await axios.post("ShoppingCart", data);
    return res;
}

export const getCart = async() => {
    var res = await axios.get("ShoppingCart");
    return res.data;
}

export const updateCartSelect = async(SelectedEditList: ShoppingCartSelectedEditDTO[]) =>{
    var res = await axios.post("ShoppingCart/CartSelected", SelectedEditList);
    return res.data;
}