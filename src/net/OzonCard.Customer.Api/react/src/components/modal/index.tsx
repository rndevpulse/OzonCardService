

import React from "react";
import "./index.css"


interface IModalProps{
    children: React.ReactNode
    title: string
    onClose: () => void
}

export function Modal({children, title, onClose} : IModalProps){
    return (
        <>
            <div className="modalBackground" onClick={onClose}></div>
            <div className="modalContent form-group">
                <h1>
                    {title}
                    <i className="material-icons red-text" onClick={onClose}>cancel</i>
                </h1>
                {children}
            </div>
        </>
    )
}