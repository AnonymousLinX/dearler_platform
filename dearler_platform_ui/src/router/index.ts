import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import Login from '../views/Login.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Login',
    component: Login
  },
  {
    path: '/main',
    name: 'Main',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/Main.vue')
  },
  {
    path: '/layoutMain',
    name: 'LayoutMain',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/LayoutMain.vue'),
    redirect: '/main',
    children:[
      {
        path: '/Main',
        name: 'Main',
        component: () => import(/* webpackChunkName: "about" */ '../views/Main.vue')
      },
      {
        path: '/productList',
        name: 'ProductList',
        component: () => import(/* webpackChunkName: "about" */ '../views/ProductList.vue')
      },
      {
        path: '/ShoppingCart',
        name: 'ShoppingCart',
        component: () => import(/* webpackChunkName: "about" */ '../views/ShoppingCart.vue')
      }
    ]
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
