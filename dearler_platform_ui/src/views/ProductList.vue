<template>
  <div>
    <!-- 搜索面板 -->
    <div class="search-pad">
      <input
        type="text"
        name=""
        id=""
        @focus="searchFocus()"
        @blur="searchBlur()"
        @input="search"
        v-model="searchText"
      />
      <button v-show="isShowSearchBtn">搜索</button>
      <button v-show="!isShowSearchBtn" @click="showRight()">筛选</button>
    </div>
    <!-- 物品大类面板 -->
    <div class="system-pad">
      <div
        v-for="belongType in belongTypes"
        :key="belongType.belongTypeNo"
        :class="[
          'system-item',
          { 'system-select': belongTypeNo == belongType.belongTypeNo },
        ]"
        @click="getSystemProduct(belongType.belongTypeNo)"
      >
        <span>{{ belongType.belongTypeName }}</span>
      </div>
    </div>
    <!-- 物品显示面板 -->
    <div class="product-list">
      <ul>
        <li v-for="product in products" :key="product.id">
          <img :src="product.productPhoto.productPhotoUrl" alt="" />
          <div>
            <p class="p-name">{{ product.productName }}</p>
            <p class="p-type">{{ product.typeName }}</p>
            <p class="p-price">&yen;{{ product.productSale.salePrice }}/张</p>
            <p class="p-cart" @click="onAddCart(product.productNo, 1)">
              <em></em>
            </p>
          </div>
        </li>
      </ul>

      <!-- 左侧物品类型面板 -->
      <div :class="['left-menu', { 'left-menu-show': isShowLeft }]">
        <div class="left-switch" @click="showLeft()">
          <img src="/img/dealerImgs/up.png" alt="" />
        </div>
        <ul>
          <li :class="{ 'left-item-select': typeSelected == '' }" @click="selectType('')">
            全部
          </li>
          <li
            v-for="productType in productTypes"
            :key="productType.typeNo"
            :class="{ 'left-item-select': typeSelected == productType.typeNo }"
            @click="selectType(productType.typeNo)"
          >
            {{ productType.typeName }}
          </li>
        </ul>
      </div>
    </div>
    <!-- 右侧侧物品属性面板 -->
    <div class="right-pad">
      <div class="list-pad">
        <ul class="f-type-list">
          <template v-for="(values, key) in productProps" :key="key">
            <li v-if="values.length > 0">
              <p>{{getPropKey(key as string, 1)}}</p>
              <ul class="f-item-list">
                <li
                  v-for="value in values"
                  :key="value"
                  @click="selectProp(getPropKey(key as string, 0), value)"
                >
                  <span
                    :class="{'prop-select': isPropSelected(getPropKey(key as string, 0), value)}"
                    >{{ value }}</span
                  >
                </li>
              </ul>
              <div class="clear-tag"></div>
            </li>
          </template>
        </ul>
      </div>
      <div class="right-edit">
        <button
          @click="confirmFilter()"
          style="background-color: rgb(188, 0, 0); color: #fff"
        >
          确定
        </button>
        <button @click="hideRight()">取消</button>
      </div>
    </div>
    <div class="cover" v-show="isShowCover" @click="hideRight()"></div>
  </div>
</template>

<script lang="ts">
import { onBeforeUnmount, onMounted, reactive, toRefs } from "vue";
import {
  getProduct,
  getBelongType,
  getType,
  getprop,
} from "@/HttpRequests/ProductListRequest";
import { IproductInfo } from "@/interfaces/IProductList";
import { useRoute, useRouter } from "vue-router";
import { addCart } from "@/HttpRequests/ShoppingCartRequest";

export default {
  setup() {
    var router = useRouter();
    var route = useRoute();

    const pageController = reactive({
      isShowLeft: false,
      isShowCover: false,
      isShowSearchBtn: false,
    });

    const productInfo: IproductInfo = reactive({
      pageIndex: 1,
      belongTypeNo: "BC",
      searchText: "",
      propSelected: {},
      tempProps: {},
      products: [],
      loading: false,
      noMore: false,
      belongTypes: [],
      productTypes: [],
      productProps: {},
      typeSelected: "",
      timer: 0,
      getProducts: async (isPushproducts) => {
        console.log(1);
        productInfo.loading = true;
        // 处理路由
        var urlParams = new URLSearchParams();
        var propParams = new URLSearchParams();
        if (productInfo.belongTypeNo)
          urlParams.append("belongTypeNo", productInfo.belongTypeNo);
        if (productInfo.typeSelected)
          urlParams.append("productType", productInfo.typeSelected);
        if (productInfo.searchText) urlParams.append("keywords", productInfo.searchText);
        for (let key in productInfo.propSelected) {
          const values = productInfo.propSelected[key];
          values.forEach((value) => {
            propParams.append(key, value);
          });
        }
        router.push(
          `/productList?${urlParams.toString() + "&&" + propParams.toString()}`
        );
        // 发送请求
        const newProducts = await getProduct({
          proptype: propParams.toString(),
          searchText: productInfo.searchText,
          blongType: productInfo.belongTypeNo,
          productType: productInfo.typeSelected ?? "",
          sort: "ProductName",
          pageIndex: productInfo.pageIndex,
          pageSize: 15,
        });
        if (Object.keys(newProducts).length === 0) {
          productInfo.noMore = true;
        }
        if (isPushproducts) {
          productInfo.products.push(...newProducts);
        } else {
          productInfo.products = newProducts;
        }
        productInfo.loading = false;
      },
      getBelongType: async () => {
        productInfo.belongTypes = await getBelongType();
      },
      // 选择大类
      getSystemProduct: async (belongTypeNo: string) => {
        // 清空当前选项
        productInfo.typeSelected = "";
        productInfo.propSelected = {};
        initIndex();

        productInfo.belongTypeNo = belongTypeNo;
        await productInfo.getProducts();
        productInfo.getTypes();
        productInfo.getProps(belongTypeNo);
      },
      getTypes: async () => {
        productInfo.productTypes = await getType(productInfo.belongTypeNo);
      },
      // 选择左侧属性
      selectType: async (typeNo: string) => {
        initIndex();
        productInfo.typeSelected = typeNo;
        await productInfo.getProducts();
      },
      getProps: async (belongTypeNo: string, typeNo?: string) => {
        productInfo.productProps = await getprop({ belongTypeNo, typeNo });
      },
      search: () => {
        productInfo.timer = setTimeout(async () => {
          clearTimeout(productInfo.timer);
          await productInfo.getProducts();
        }, 1000);
      },
      selectProp: async (proptitle: string, prop: string) => {
        // 将未选中的属性加入数组, 将已选中的属性从数组删除
        const selectList = productInfo.tempProps[proptitle] || [];
        const index = selectList.indexOf(prop);
        if (index > -1) {
          selectList.splice(index, 1);
        } else {
          selectList.push(prop);
        }
        productInfo.tempProps[proptitle] = selectList;
      },
      // 使用临时变量tempProps检查是否被选中
      isPropSelected: (proptitle: string, prop: string) => {
        const selectList = productInfo.tempProps[proptitle] || [];
        return selectList.includes(prop);
      },
      onAddCart: (productNo, productNum) => {
        const customerNo = localStorage["customerNo"];
        addCart({
          CustomerNo: customerNo,
          ProductNo: productNo,
          ProductNum: productNum,
        });
      },
    });

    const showLeft = () => {
      pageController.isShowLeft = !pageController.isShowLeft;
    };
    const searchFocus = () => {
      pageController.isShowSearchBtn = true;
    };
    const searchBlur = () => {
      pageController.isShowSearchBtn = false;
    };
    const confirmFilter = () => {
      pageController.isShowCover = false;
      const rightPad = document.querySelector(".right-pad");
      if (rightPad instanceof HTMLElement) {
        rightPad.style.right = "-85%";
      }

      initIndex();
      productInfo.propSelected = JSON.parse(JSON.stringify(productInfo.tempProps));
      productInfo.tempProps = {};
      productInfo.getProducts();
    };
    const showRight = () => {
      // 打开右侧属性筛选时, 将propSelected数据拷贝至tempProps
      if (Object.keys(productInfo.tempProps).length === 0) {
        productInfo.tempProps = JSON.parse(JSON.stringify(productInfo.propSelected));
      }
      pageController.isShowCover = true;
      const rightPad = document.querySelector(".right-pad");
      if (rightPad instanceof HTMLElement) {
        rightPad.style.right = "0";
      }
    };
    const hideRight = () => {
      pageController.isShowCover = false;
      const rightPad = document.querySelector(".right-pad");
      if (rightPad instanceof HTMLElement) {
        rightPad.style.right = "-85%";
      }
      productInfo.tempProps = {};
      productInfo.getProducts();
    };

    // 恢复页面选项
    const restorationAddress = async () => {
      productInfo.propSelected = {};
      const query = route.query;
      if (typeof query.belongTypeNo === "string") {
        productInfo.belongTypeNo = query.belongTypeNo;
      }
      if (typeof query.productType === "string") {
        productInfo.typeSelected = query.productType;
      }
      if (typeof query.keywords === "string") {
        productInfo.searchText = query.keywords;
      }
      // 处理url获取属性设置
      const propsQueryStr = route.fullPath.split("&&")[1];
      // URLSearchParams处理属性编码赋值给propSelected
      const propParams = new URLSearchParams(propsQueryStr);
      for (const [key, value] of propParams.entries()) {
        if (!productInfo.propSelected[key]) {
          productInfo.propSelected[key] = [];
        }
        productInfo.propSelected[key].push(value);
      }
      await productInfo.getProducts();
    };

    const getPropKey = (proptitle: string, num: number) => {
      return String(proptitle).split("|")[num].split("List")[0].trim();
    };

    const initIndex = () => {
      productInfo.pageIndex = 1;
      productInfo.noMore = false;
      // 页面回到最上方
      window.scrollTo({ top: 0 });
    };

    // 监听页面滚动事件
    const handleScroll = () => {
      const scrollTop = window.scrollY || document.documentElement.scrollTop;
      const windowHeight = window.innerHeight;
      const pageHeight = document.documentElement.scrollHeight;
      if (
        scrollTop + windowHeight > pageHeight - 100 &&
        !productInfo.loading &&
        !productInfo.noMore
      ) {
        onPageChange();
      }
    };

    const onPageChange = () => {
      productInfo.pageIndex++;
      productInfo.getProducts(true);
    };

    onMounted(async () => {
      await restorationAddress();
      await productInfo.getBelongType();
      await productInfo.getTypes();
      await productInfo.getProps(productInfo.belongTypeNo);
      // 添加滚轮监听器
      window.addEventListener("scroll", handleScroll);
    });

    onBeforeUnmount(() => {
      window.removeEventListener("scroll", handleScroll);
    });

    return {
      ...toRefs(pageController),
      ...toRefs(productInfo),
      showLeft,
      searchFocus,
      searchBlur,
      confirmFilter,
      showRight,
      hideRight,
      getPropKey,
    };
  },

  /*mounted() {
        this.$store.comm
        this.$store.dispatch('setFootMenuIndexAsync', 1);
    }, */
};
</script>

<style lang="scss" scoped>
.i-search:after {
  background-color: #b70101 !important;
}

.search-pad {
  z-index: 10;
  position: fixed;
  width: 100%;
  padding: 6px 20px;
  background-color: #f0f0f0;
  display: flex;

  input {
    height: 28px;
    box-sizing: border-box;
    border: 1px solid #ddd;
    border-radius: 3px;
    flex: 1;
    outline: none;
  }

  button {
    background-color: transparent;
    width: 56px;
    border: 0 none;
    font-size: 14px;
    font-weight: bold;
    color: #333;
    outline: none;
  }
}

.system-pad {
  z-index: 10;
  top: 40px;
  position: fixed;
  width: 100%;
  background-color: #fff;
  display: flex;

  .system-item {
    flex: 1;
    text-align: center;
    border-bottom: 1px #ddd solid;
    border-right: 1px transparent solid;
    border-left: 1px transparent solid;

    span {
      border: 0 none !important;
      background-color: #f0f2f5;
      margin: 6px 5px;
      font-size: 12px;
      font-weight: normal;
      text-align: center;
      border-radius: 4px;
      padding: 6px 0;
      display: block;
      height: 22px;
      line-height: 12px;
    }
  }

  .system-select {
    border-bottom: 1px transparent solid;
    border-right: 1px #ddd solid;
    border-left: 1px #ddd solid;

    span {
      background-color: transparent;
    }
  }
}

.product-list {
  padding-top: 75px;
  ul {
    background-color: #fff;

    li {
      list-style: none;
      height: 88px;
      padding-left: 108px;
      position: relative;

      img {
        height: 66px;
        width: 66px;
        background-color: #ccc;
        position: absolute;
        left: 28px;
        top: 11px;
      }

      div {
        overflow: hidden;
        padding: 10px 0;
        border-bottom: 1px solid #f0f0f0;
        padding-bottom: 12px;
        text-align: left;
        .p-name {
          font-size: 13px;
        }

        .p-type {
          font-size: 12px;
          color: #666;
          margin-top: 8px;
        }

        .p-price {
          font-size: 13px;
          color: #f23030;
          margin-top: 8px;
        }

        .p-cart {
          float: right;
          margin-right: 20px;
          background-color: #b70101;
          background-image: url("http://localhost:8080/img/icons-png/shoppingCar-white.png");
          background-repeat: no-repeat;
          background-position: center;
          background-position-x: 45%;
          border-radius: 50px;
          height: 20px;
          width: 40px;
        }
      }
    }
  }

  .left-menu {
    position: fixed;
    height: calc(100% - 116px);
    left: -106px;
    width: 125px;
    background-color: #fff;
    top: 76px;
    border-radius: 0 18px 0 0;
    border: 1px solid #d7d7d7;
    overflow: hidden;
    transition: 0.5s;
    margin-bottom: 120px;

    .left-switch {
      width: 20px;
      background-color: #fff;
      position: absolute;
      right: 0;
      height: 100%;

      img {
        position: absolute;
        top: 42%;
        left: 2px;
        width: 20px;
        transform: rotate(90deg);
        transition: 0.5s;
      }
    }

    ul {
      position: absolute;
      height: 100%;
      width: 106px;
      background-color: #f0f0f0;
      overflow: auto;

      li {
        width: 106px;
        height: 50px;
        text-align: center;
        line-height: 50px;
        border-bottom: 1px solid #d7d7d7;
        padding: 0;
        font-size: 12px;
        color: #333;
      }

      li.left-item-select {
        background-color: #fff;
      }
    }
  }

  .left-menu-show {
    left: 0;

    .left-switch {
      img {
        transform: rotate(-90deg);
      }
    }
  }
}

.right-pad {
  position: fixed;
  /* right: -85%; */
  right: -85%;
  top: 0;
  width: 85%;
  height: 100%;
  background-color: #f7f7f7;
  z-index: 103;
  transition: 580ms;
  z-index: 101;
  overflow: auto;

  ul {
    list-style: none;
    overflow: hidden;
  }

  .list-pad {
    overflow: auto;
    height: 100%;
    padding-bottom: 40px;
    .f-type-list {
      overflow: hidden;

      > li {
        padding: 10px;
        background-color: #fff;
        margin-bottom: 10px;

        .f-item-list {
          overflow: auto;
          display: flex;
          flex-wrap: wrap;

          li {
            flex-basis: 33.3%;

            span {
              display: block;
              margin-top: 10px;
              margin-right: 10px;
              background: #eee;
              border: 1px solid #eee;
              padding: 5px 0;
              text-align: center;
              border-radius: 6px;
              font-size: 13px;
            }

            .prop-select {
              border: 1px solid red;
              background: #fff;
              color: red;
            }
          }
        }

        p {
          font-size: 14px;
        }
      }
    }
  }

  .right-edit {
    position: absolute;
    bottom: 0;
    right: 0;
    width: 100%;

    button {
      float: left;
      height: 40px;
      width: 50%;
      line-height: 40px;
      text-align: center;
      border: 0px none;
    }
  }
}

.cover {
  z-index: 11;
  position: fixed;
  height: 100%;
  width: 100%;
  left: 0;
  top: 0;
  background-color: rgba(51, 51, 51, 0.36);
}
</style>
