import axios from "axios"

export const getCartNo = async() => {
    var res = await axios.get("/ShoppingCart/num");
    return res.data;
}