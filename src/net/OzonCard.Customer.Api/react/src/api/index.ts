import axios from 'axios'
import {Slide, toast} from "react-toastify";
import AuthService from "../services/AuthService";
import {useNavigate} from "react-router-dom";
import {IAuth} from "../models/auth/IAuth";

const url =
    'https://localhost:5180/api/v1';
    //'https://lp.corpcards.ru/api/v1'
    // process.env.NODE_ENV || process.env.NODE_ENV  === 'development'
    //     ? 'https://localhost:5180/api/v1'
    //     // : 'https://ozon.pulse2.keenetic.link/api/v1';
    //     : 'https://ozon.kolur.keenetic.link/api/v1';
const api = axios.create({
    withCredentials: true,
    baseURL: url
})

api.interceptors.request.use((config) => {
    config.headers!.Authorization = `Bearer ${localStorage.getItem('token')}`
    return config
})

api.interceptors.response.use(config => {
    return config;
}, async error => {
    const originalRequest = error.config;
    if (error.response.status === 401 && error.config && !originalRequest._isRetry) {
        originalRequest._isRetry = true;
        try {
            // const response = await api.get<IAuth>('/auth/refresh');
            // const response = await axios.post<IAuthResponse>(`${API_URL}/auth/refresh`, { withCredentials: true })
            // localStorage.setItem('token', response.data.access);
            console.log('try refreshing token');
            const response = await api.get<IAuth>('/auth/refresh?token=' + localStorage.getItem('refresh'),{
                headers:{
                    Authorization: `Bearer ${localStorage.getItem('token')}`
                }
            });
            localStorage.setItem('token', response.data.access);
            localStorage.setItem('refresh', response.data.refresh);
            return api.request(originalRequest);
        }
        catch (e) {
            console.log('no authorization')
            localStorage.clear();
            return api.request(originalRequest);
        }

    }
    console.log(error.response.data.detail)
    toast.error(error.response.data.detail, {
        position: "bottom-right",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        theme: "light",
        transition: Slide,
    });

})

export default api