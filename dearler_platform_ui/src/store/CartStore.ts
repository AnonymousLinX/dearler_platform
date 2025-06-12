import { ref } from 'vue'
import { defineStore } from 'pinia'

export const useCartStore = defineStore('cartStore', () => {
  const shoppingCartNum = ref(0)
  function SetCartNum(num: number): void {
    shoppingCartNum.value = num;
  }
  return {
    shoppingCartNum,
    SetCartNum
  }
})