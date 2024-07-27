import {Slide, toast, ToastOptions} from "react-toastify";

const options: ToastOptions = {
    position: "bottom-right",
    autoClose: 3000,
    hideProgressBar: false,
    closeOnClick: true,
    pauseOnHover: true,
    draggable: true,
    progress: undefined,
    theme: "light",
    transition: Slide,
}

class CustomToast{

    show(message:string, type?:"warning" | "info" | "error" | null | undefined){
        switch (type){
            case "info":
                toast.info(message, options);
                return;
            case "warning":
                toast.warn(message, options);
                return;
            case "error":
                toast.error(message, options);
                return;
            default:
                toast(message, options);
        }
    }
}

export function useToast(): CustomToast{ return new CustomToast()};



