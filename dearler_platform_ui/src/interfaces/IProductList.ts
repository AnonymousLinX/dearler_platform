import { getProduct } from "@/HttpRequests/ProductListRequest";

export interface IProductInputDto{
    proptype: string,
    searchText?: string,
    blongType: string,
    sort: string,
    pageIndex: number;
    pageSize: number;
    productType: string;
}

export interface IproductInfo{
    pageIndex: number;
    belongTypeNo: string;
    searchText: string;
    propSelected: IProductProps;
    tempProps: IProductProps;
    products: ProductDTO[];
    loading: boolean;
    noMore: boolean;
    belongTypes: IbelongType[];
    productTypes: IProductType[];
    productProps: IProductProps;
    typeSelected: string,
    timer: number;
    // getProducts: (belongTypeNo: string, productType?: string, searchText?:string[]) => Promise<void>;
    getProducts: (isPushproducts?: boolean) => Promise<void>;
    getBelongType: () => Promise<void>;
    getSystemProduct: (belongTypeNo: string) => void;
    getTypes: () =>  void;
    selectType:(typeNo:string) => Promise<void>;
    getProps: (belongTypeNo:string, typeNo?:string) => void;
    search: () => void;
    selectProp: (proptitle:string, prop: string) => void;
    isPropSelected: (proptitle: string, prop: string) => void;
    onAddCart: (productNo:string, productNum:number) => void;
}

export interface IProductPropInputeDto{
    belongTypeNo: string;
    typeNo?: string;
}

export interface ProductDTO{
    id: number;
    sysNo: string;
    productNo: string;
    productName: string;
    typeNo: string;
    typeName: string;
    productPp?: string;
    productXh?: string;
    productCz?: string;
    productHb?: string;
    productHd?: string;
    productGy?: string;
    productHs?: string;
    productMc?: string;
    productDj?: string;
    productCd?: string;
    productGg?: string;
    productYs?: string;
    productNote?: string;
    productBzgg?: string;
    belongTypeNo: string;
    belongTypeName: string;
    unitNo: string;
    unitName: string;
    productPhoto: ProductPhoto;
    productSale: ProductSale;
    
}

// 产品图片信息
interface ProductPhoto {
    id: number;
    sysNo: string;
    productNo: string;
    productPhotoUrl: string;
}


// 产品销售信息
interface ProductSale {
    id: number;
    sysNo: string;
    productNo: string;
    stockNo: string;
    salePrice: number;
}

interface IbelongType {
    systNo: string;
    belongTypeNo: string;
    belongTypeName: string;
}

interface IProductType {
    typeNo: string;
    typeName: string;
}

interface IProductProps {
    [PropName: string]: string[];
}

export interface IShoppingCartInputDto{
    CustomerNo: string;
    ProductNo: string;
    ProductNum: number;
}