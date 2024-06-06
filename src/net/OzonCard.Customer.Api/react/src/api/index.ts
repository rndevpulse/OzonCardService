import axios from 'axios'
import {IAuth} from "../models/auth/IAuth";
import {Bounce, Slide, toast} from "react-toastify";

// export const API_URL = 'https://localhost:5180/api/v1'
// export const API_URL = 'https://ozon.pulse2.keenetic.link/api/v1'
export const API_URL = 'https://ozon.pulse2.keenetic.link/api/v1'

const api = axios.create({
    withCredentials: true,
    baseURL: API_URL
})


api.interceptors.request.use((config) => {
    config.headers!.Authorization = `Bearer ${localStorage.getItem('token')}`
    return config
})

api.interceptors.response.use(config => {
    return config;
}, async error => {
    const originalRequest = error.config;
    if (error.response.status == 401 && error.config && !originalRequest._isRetry) {
        originalRequest._isRetry = true;
        try {
            const response = await api.get<IAuth>('/auth/refresh');
            // const response = await axios.post<IAuthResponse>(`${API_URL}/auth/refresh`, { withCredentials: true })
            localStorage.setItem('token', response.data.access);
            //console.log(response);
            return api.request(originalRequest);
        }
        catch (e) {
            //console.log('no autorization')
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
        theme: "dark",
        transition: Slide,
    });

})

export default api