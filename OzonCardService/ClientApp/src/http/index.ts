import axios from 'axios'
import { IAuthResponce } from '../models/IAuthResponse'

export const API_URL = 'https://192.168.1.100:5401/api'
//export const API_URL = 'https://ozon.pulse.keenetic.link/api'

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
            const response = await axios.post<IAuthResponce>(`${API_URL}/auth/refresh`, { withCredentials: true })
            localStorage.setItem('token', response.data.token);
            //console.log(response);
            return api.request(originalRequest);
        }
        catch (e) {
            //console.log('no autorization')
        }
        
    }
    throw error;
})

export default api