import axios from "axios";
import { IProductInputDto, IProductPropInputeDto } from "@/interfaces/IProductList";

export const getProduct = async(data: IProductInputDto) => {
    var res = await axios.get("Product", { params: data });
    return res.data;
}

export const getBelongType = async() => {
    var res = await axios.get("Product/BlongType");
    return res.data;
}

export const getType = async(belongTypeNo: string) => {
    var res = await axios.get("Product/Type", { params: {belongTypeNo} });
    return res.data;
}

export const getprop = async(data: IProductPropInputeDto) => {
    var res = await axios.get("Product/props", { params: data });
    return res.data; 
}
