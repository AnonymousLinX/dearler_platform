<template>
  <div>
    <div class="user-box">
      <div class="user-info">
        <div class="user-head">
          <img src="/img/dealerImgs/picUser.png" alt="" class="" />
        </div>
        <p class="user-name">Lin</p>
        <p>销售员：员工A</p>
      </div>
    </div>
    <div class="menu-item">
      <img src="/img/dealerImgs/purchase_order.png" alt="" />
      <div class="menu-info">
        <p class="m-title">我的订单</p>
        <p class="m-info">未完成的订单：8个</p>
      </div>
    </div>
    <div class="menu-item">
      <img src="/img/dealerImgs/shopping212.png" alt="" />
      <div class="menu-info">
        <p class="m-title">购物车</p>
        <p class="m-info">购物车中有：{{ store.shoppingCartNum }}个待下单的物品</p>
      </div>
    </div>
    <div class="menu-item">
      <img src="/img/dealerImgs/door.png" alt="" />
      <div class="menu-info">
        <p class="m-title">退出账号</p>
        <p class="m-info">退出当前帐号</p>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { onMounted } from "vue";
import { getCartNo } from "../HttpRequests/MainRequest";
import { useCartStore } from "../store/CartStore";
export default {
  setup() {
    const store = useCartStore();
    const onGetCartNum = async () => {
      var customerNo = localStorage["customerNo"];
      store.SetCartNum(await getCartNo());
    };
    onMounted(async () => {
      await onGetCartNum();
    });
    return { store };
  },
};
</script>

<style lang="scss" scoped>
.user-box {
  padding: 10px;
  background-color: #fff;

  .user-info {
    padding: 25px 0 25px 80px;
    height: 100px;
    border-radius: 10px;
    position: relative;
    background: -webkit-linear-gradient(left, #b70101, #f20505);

    p {
      color: #fff;
      text-align: left;
      font-size: 14px;
      margin-bottom: 16px;
      color: hsla(0, 0%, 100%, 0.7);
    }

    p.user-name {
      letter-spacing: 2px;
      font-weight: bold;
      font-size: 16px;
      color: #fff;
    }

    .user-head {
      width: 40px;
      height: 40px;
      border-radius: 40px;
      border: 2px solid #fff;
      overflow: hidden;
      background-color: #fff;
      position: absolute;
      top: 36px;
      left: 20px;

      img {
        width: 40px;
        height: 40px;
      }
    }
  }
}

.menu-item {
  height: 60px;
  background-color: #fff;
  margin-top: 10px;
  padding: 6px;
  position: relative;
  padding-left: 60px;

  img {
    position: absolute;
    top: 20px;
    left: 20px;
  }

  .menu-info {
    p.m-title {
      margin-top: 10px;
      font-weight: bold;
      font-size: 15px;
    }
    p.m-info {
      margin-top: 8px;
      font-size: 12px;
    }
  }
}
</style>
