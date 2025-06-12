<template>
  <div>
    <div class="cart-list">
      <ul>
        <li v-for="type in types" :key="type.typeNo">
          <p>
            <i
              :class="{ 'cart-select': isTypeAllSelected(type) }"
              @click="onToggleAllSelect(type)"
              >✔</i
            >
            <span>{{ defaultTypeOnNull(type.typeName) }}</span>
          </p>
          <template v-for="cart in shoppingcarts">
            <div v-if="cart.productDto?.typeNo == type.typeNo" :key="cart.id">
              <i :class="{ 'cart-select': cart.cartSelected }" @click="onSelectCart(cart)"
                >✔</i
              >
              <img src="" alt="" />
              <p class="p-name">{{ cart.productDto?.productName }}</p>
              <p class="p-price">&yen;{{ cart.productDto.productSale.salePrice }}</p>
              <p class="p-num">
                <span class="sub-num" @click="onSubNum(cart)">-</span>
                <input
                  type="text"
                  name=""
                  id=""
                  v-model="cart.productNum"
                  min="1"
                  @input="validateNumber(cart)"
                />
                <span class="add0num" @click="onAddNum(cart)">+</span>
                <b>块</b>
              </p>
            </div>
          </template>
        </li>
      </ul>
    </div>
    <div class="total-pad">
      <i :class="{ 'cart-select': showSelectAllButton() }" @click="onSelectAllButton()"
        >✔</i
      >
      <span>全选</span>
      <span>
        合计：&yen; <b>{{ computedTotalPrice }}</b>
      </span>
      <button>确定下单</button>
    </div>
  </div>
</template>

<script lang="ts">
import { getCart, updateCartSelect } from "@/HttpRequests/ShoppingCartRequest";
import {
  IshoppingCartInfo,
  ShoppingCartDTO,
  ShoppingCartSelectedEditDTO,
} from "@/interfaces/IShoppingCart";
import { onMounted, reactive, toRefs, computed } from "vue";
import ProductList from "./ProductList.vue";

export default {
  setup() {
    const shoppingCartInfo: IshoppingCartInfo = reactive({
      shoppingcarts: [],
      types: [],
      typeSelected: false,
      onAddNum: async (cart) => {
        cart.productNum++;
        const shoppingCartEdit: ShoppingCartSelectedEditDTO = {
          cartGuid: cart.cartGuid,
          cartSelected: cart.cartSelected,
          productNum: cart.productNum,
        };
        await updateCartSelect([shoppingCartEdit]);
      },
      onSubNum: async (cart) => {
        if (cart.productNum > 1) {
          cart.productNum--;
        }
        const shoppingCartEdit: ShoppingCartSelectedEditDTO = {
          cartGuid: cart.cartGuid,
          cartSelected: cart.cartSelected,
          productNum: cart.productNum,
        };
        await updateCartSelect([shoppingCartEdit]);
      },
      onGetShoppingCarts: async () => {
        var customerNo = localStorage["customerNo"];
        var res = await getCart();
        shoppingCartInfo.shoppingcarts = res.carts;
        shoppingCartInfo.types = res.type;
      },
      onSelectCart: async (cart) => {
        const shoppingCartEdit: ShoppingCartSelectedEditDTO = {
          cartGuid: cart.cartGuid,
          cartSelected: !cart.cartSelected,
          productNum: cart.productNum,
        };
        await updateCartSelect([shoppingCartEdit]);
        cart.cartSelected = !cart.cartSelected;
      },
      isTypeAllSelected: (type) => {
        const SameTypeCarts = shoppingCartInfo.shoppingcarts.filter(
          (m) => m.productDto.typeNo == type.typeNo
        );
        const isAllSelected = SameTypeCarts.every((m) => m.cartSelected);
        return isAllSelected;
      },
      onToggleAllSelect: async (type) => {
        const SameTypeCarts = shoppingCartInfo.shoppingcarts.filter(
          (m) => m.productDto.typeNo == type.typeNo
        );
        const isAllSelected = SameTypeCarts.every((m) => m.cartSelected);
        const targetSelected = !isAllSelected;
        const shoppingCartEdits: ShoppingCartSelectedEditDTO[] = [];
        SameTypeCarts.forEach((p) => {
          const shoppingCartEdit: ShoppingCartSelectedEditDTO = {
            cartGuid: p.cartGuid,
            cartSelected: targetSelected,
            productNum: p.productNum,
          };
          shoppingCartEdits.push(shoppingCartEdit);
        });
        await updateCartSelect(shoppingCartEdits);
        SameTypeCarts.forEach((m) => (m.cartSelected = targetSelected));
      },
      computedTotalPrice: computed(() => {
        let sum = 0;
        shoppingCartInfo.shoppingcarts.forEach((m) => {
          if (m.cartSelected) {
            sum += m.productNum * m.productDto.productSale.salePrice;
          }
        });
        return sum;
      }),
      showSelectAllButton: () => {
        return shoppingCartInfo.shoppingcarts.every((m) => m.cartSelected);
      },
      onSelectAllButton: async () => {
        const targetSelectState = !shoppingCartInfo.showSelectAllButton();
        const shoppingCartEdits: ShoppingCartSelectedEditDTO[] = [];
        shoppingCartInfo.shoppingcarts.forEach((p) => {
          const shoppingCartEdit: ShoppingCartSelectedEditDTO = {
            cartGuid: p.cartGuid,
            cartSelected: targetSelectState,
            productNum: p.productNum,
          };
          shoppingCartEdits.push(shoppingCartEdit);
        });
        await updateCartSelect(shoppingCartEdits);
        shoppingCartInfo.shoppingcarts.forEach(
          (m) => (m.cartSelected = targetSelectState)
        );
      },
    });
    const defaultTypeOnNull = (type: string) => {
      return type ?? "未分类产品";
    };
    const validateNumber = (cart: ShoppingCartDTO) => {
      let num = Number(cart.productNum);
      if (isNaN(num) || num < 1) {
        num = 1;
      }
      if (num > 99) {
        num = 99;
      }
      cart.productNum = num;
    };
    onMounted(async () => {
      await shoppingCartInfo.onGetShoppingCarts();
    });
    return {
      ...toRefs(shoppingCartInfo),
      defaultTypeOnNull,
      validateNumber,
    };
  },
};
</script>

<style lang="scss" scoped>
.cart-list {
  text-align: left;
  ul {
    margin-bottom: 108px;

    li {
      background-color: #fff;
      margin-bottom: 12px;

      > p {
        padding-left: 46px;
        position: relative;
        height: 46px;
        border-bottom: 1px solid #ddd;

        i {
          border: 1px solid #a9a9a9;
          width: 18px;
          height: 18px;
          line-height: 18px;
          border-radius: 18px;
          position: absolute;
          left: 13px;
          top: 13px;
          text-align: center;
          font-size: 12px;
          color: #fff;
          font-style: normal;
        }

        i.cart-select {
          background-color: crimson;
          border: 1px solid crimson;
        }

        span {
          display: inline-block;
          border-left: 3px solid crimson;
          height: 28px;
          margin: 9px 0;
          padding-left: 8px;
          line-height: 30px;
        }
      }

      div {
        padding-left: 46px;
        position: relative;
        height: 98px;
        padding: 8px 14px 8px 148px;

        i {
          border: 1px solid #a9a9a9;
          width: 18px;
          height: 18px;
          line-height: 18px;
          border-radius: 18px;
          position: absolute;
          left: 13px;
          top: 28px;
          text-align: center;
          font-size: 12px;
          color: #fff;
          font-style: normal;
        }

        i.cart-select {
          background-color: crimson;
          border: 1px solid crimson;
        }

        img {
          width: 68px;
          height: 68px;
          background-color: #ccc;
          position: absolute;
          left: 58px;
          top: 20px;
        }

        p.p-name {
          font-size: 13px;
          margin-top: 10px;
          height: 30px;
        }
        p.p-price {
          font-size: 13px;
          height: 20px;
          color: crimson;
        }
        p.p-num {
          text-align: right;
          padding-right: 20px;

          span {
            display: inline-block;
            width: 18px;
            height: 18px;
            border: 1px solid crimson;
            color: crimson;
            border-radius: 9px;
            text-align: center;
            line-height: 18px;
          }

          input {
            width: 28px;
            border: none 0px;
            outline: none;
            text-align: center;
          }

          b {
            font-weight: normal;
            margin-left: 10px;
            font-size: 13px;
          }
        }
      }
    }
  }
}

.total-pad {
  height: 58px;
  width: 100%;
  background-color: #383838;
  position: fixed;
  left: 0;
  bottom: 40px;

  i {
    display: inline-block;
    border: 1px solid #a9a9a9;
    width: 18px;
    height: 18px;
    line-height: 18px;
    border-radius: 18px;
    background-color: #fff;
    margin-left: 13px;
    margin-top: 20px;
    vertical-align: bottom;
    height: 18px;
    text-align: center;
    font-size: 12px;
    color: #fff;
    font-style: normal;
  }

  i.cart-select {
    background-color: crimson;
    border: 1px solid crimson;
  }

  span {
    color: #fff;
    margin-left: 6px;
    font-size: 13px;

    b {
      font-size: 15px;
    }
  }

  button {
    float: right;
    height: 58px;
    width: 120px;
    border: 0 none;
    background-color: #ddd;
    color: #aaa;
    font-size: 15px;
    font-weight: bold;
  }
}
</style>
