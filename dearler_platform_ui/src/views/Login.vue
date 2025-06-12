<template>
  <div>
    <div class="login-pad">
      <h2>
        <img src="/img/icons/Logo2.png" alt="" />
        经销商平台
      </h2>
      <p>
        <input type="text" placeholder="用户名" v-model="userNo" />
      </p>
      <p>
        <input
          type="password"
          placeholder="密码"
          v-model="password"
          @keyup.enter="login"
        />
      </p>
      <button @click="login">→</button>
    </div>
    <div class="login-bottom">&copy; Website</div>
  </div>
</template>

<script lang="ts">
import { reactive, toRefs } from "vue";
import { ILoginInfo } from "@/interfaces/ILogin";
import { userLogin } from "@/HttpRequests/LoginRequest";
import { useRouter } from "vue-router";
export default {
  setup() {
    var router = useRouter();
    const loginInfo: ILoginInfo = reactive({
      userNo: "",
      password: "",
      login: async () => {
        if (!loginInfo.userNo || !loginInfo.password) {
          alert("用户名和密码不能为空!");
          return;
        }
        try {
          var res = await userLogin({
            CustomerNo: loginInfo.userNo,
            password: loginInfo.password,
          });
          if (res.status == 200) {
            localStorage["customerNo"] = loginInfo.userNo;
            localStorage["token"] = res.data;
            router.push("/LayoutMain");
          }
        } catch (error: any) {
          if (error.response?.status == 401) {
            alert("用户名或密码错误");
          } else {
            alert("发生未知错误，请稍后再试");
            console.error(error); // 可选：用于开发调试
          }
          return;
        }
      },
    });
    return {
      ...toRefs(loginInfo),
    };
  },
};
</script>

<style lang="scss" scoped>
.login-pad {
  text-align: center;
  width: 60%;
  margin: auto;
  margin-top: 26%;
  h2 {
    font-weight: normal;
    margin-bottom: 30px;
    img {
      display: inline-block;
      width: 36px;
      height: 36px;
      background: transparent;
      border-radius: 18px;
      vertical-align: -9px;
    }
  }
  p {
    width: 100%;
    margin-top: 20px;
    input {
      width: 100%;
      box-sizing: border-box;
      height: 36px;
      border-radius: 18px;
      border: 0 none;
      background-color: #f0f0f0;
      text-align: center;
    }
  }
  button {
    margin-top: 36px;
    width: 60px;
    height: 60px;
    border-radius: 30px;
    border: 0 none;
    background-color: rgb(79, 137, 245);
    color: #fff;
    font-size: 26px;
    font-weight: bold;
  }
}
.login-bottom {
  position: absolute;
  bottom: 10px;
  text-align: center;
  width: 100%;
  font-size: 14px;
}
</style>
