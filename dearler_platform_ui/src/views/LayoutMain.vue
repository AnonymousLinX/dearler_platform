<template>
  <div>
    <router-view />
    <div class="foot-menu-pad">
      <div class="foot-menu">
        <router-link to="/productList" v-slot="{ navigate }" custom>
          <div class="foot-item" @click="navigate">
            <b :class="['i-search', 'f-menu-sel']"></b>
          </div>
        </router-link>
        <router-link to="/shoppingCart" v-slot="{ navigate }" custom>
          <div class="foot-item" @click="navigate">
            <b :class="['i-cart', 'f-menu-sel']">
              <i>{{ store.shoppingCartNum }}</i>
            </b>
          </div>
        </router-link>
        <router-link to="/main" v-slot="{ navigate }" custom>
          <div class="foot-item" @click="navigate">
            <b :class="['i-user', 'f-menu-sel']"></b>
          </div>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { useCartStore } from "../store/CartStore";
export default {
  setup() {
    const store = useCartStore();
    return { store };
  },
};
</script>

<style lang="scss" scoped>
.foot-menu-pad {
  height: 40px;
  .foot-menu {
    position: fixed;
    height: 40px;
    background-color: #fff;
    width: 100%;
    left: 0;
    bottom: 0;
    display: flex;

    .foot-item {
      flex: 1;
      text-align: center;
      height: 40px;
      line-height: 40px;
      position: relative;

      b {
        background-color: #acacac;
        width: 26px;
        height: 26px;
        background-size: 16px;
        background-repeat: no-repeat;
        background-position: center;
        border-radius: 13px;
        margin-top: 6px;
        display: inline-block;
        position: relative;

        i {
          position: absolute;
          font-size: 12px;
          color: #fff;
          background-color: red;
          padding: 1px 3px;
          text-align: center;
          font-style: normal;
          display: inline;
          top: -5px;
          right: -12px;
          border-radius: 12px;
          line-height: 12px;
          font-weight: normal;
        }
      }

      b.i-search {
        background-image: url("/public/img/icons-png/search-white.png");
      }

      b.i-cart {
        background-image: url("/public/img/icons-png/shoppingCar-white.png");
      }

      b.i-user {
        background-image: url("/public/img/icons-png/user-white.png");
      }

      .f-menu-sel {
        background-color: #b70101;
      }
    }
  }
}
</style>
