import axios from "axios";
import { useRouter } from "vue-router";

axios.interceptors.request.use(
    config => {
        // 判断是否存在token值, 如果存在则在请求前给header加上token
        if(localStorage["token"]){
            config.headers.Authorization = `Bearer ${localStorage["token"]}` // 请求头加上token
        }
        return config;
    },
    error => {
        return Promise.reject(error)
    }
)

axios.interceptors.response.use(
    response => {
        return response;
    },
    error => {
        switch(error.response.status){
            case 401:{
                var router = useRouter();
                router.push('/Login')
            }
        }
    }
)