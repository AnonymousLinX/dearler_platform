import { ComputedRef } from "vue";
import { ProductDTO } from "./IProductList";

export interface IshoppingCartInfo{
    shoppingcarts: ShoppingCartDTO[];
    types:types[],
    // totalPrice: number;
    typeSelected: boolean;
    onAddNum: (cart: ShoppingCartDTO) => void;
    onSubNum: (cart: ShoppingCartDTO) => void;
    onGetShoppingCarts: () => Promise<void>;
    onSelectCart: (cart: ShoppingCartDTO) => void;
    isTypeAllSelected: (type: types) => boolean;
    onToggleAllSelect: (type: types) => void;
    computedTotalPrice: any;
    showSelectAllButton: () => boolean;
    onSelectAllButton: () => void;
}
export interface ShoppingCartDTO {
    id: number;
    cartGuid: string;
    customerNo: string;
    productNo: string;
    productNum: number;
    cartSelected: boolean;
    productDto: ProductDTO;
}
interface types {
    typeNo: string;
    typeName: string;
}
export interface ShoppingCartSelectedEditDTO {
    cartGuid: string;
    cartSelected: boolean;
    productNum: number;
}