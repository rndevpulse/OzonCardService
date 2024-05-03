import {IFile} from "../../models/file/IFile";
import * as React from "react";


interface IFileProps {
    file: IFile,
    onRemove: (id:string) => void
    onSave: (e: React.MouseEvent<HTMLElement, MouseEvent>, url: string, name: string) => void
}


export function File({file, onRemove, onSave}: IFileProps) {

    return (
        <li className="file" key={file.url}>
            <label>
                <span onClick={(e) => onSave(e, file.url, `${file.name}`)}>
                    {file.name} | {new Date(file.created).toLocaleString()}
                </span>
                <span>
                    <i className="material-icons blue-text"
                       onClick={(e) => onSave(e, file.url, `${file.name}`)}>
                        file_download
                    </i>
                    <i className="material-icons red-text"
                       onClick={() => onRemove(file.url)}>
                        delete
                    </i>
                </span>
            </label>
        </li>
    );
}
