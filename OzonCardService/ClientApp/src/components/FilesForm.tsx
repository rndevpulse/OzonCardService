import { FC, useContext, useState } from 'react';
import * as React from 'react'
import { useEffect } from 'react';
import FileService from '../services/FileServise';
import { IFileResponse } from '../models/IFileResponse';

const FilesForm: FC = () => {
    const [files, setFiles] = useState<IFileResponse[]>([])

    async function getFiles() {
        const response = await FileService.getMyFiles()
        console.log(response.data)
        setFiles(response.data);
    }
    async function onRemoveFile(url: string) {
        console.log('onRemoveFile: ', url)

        await FileService.removeFile(url);
        await getFiles();
    }
    
    async function downloadHandler(e: React.MouseEvent<HTMLElement, MouseEvent>, url: string, name: string) {
        console.log('downloadHandler', url, name)
        e.stopPropagation()
        FileService.downloadFile(url)
            .then(response => {
                const type = response.headers['content-type']
                const blob = new Blob([response.data], { type: type })
                const _url = window.URL.createObjectURL(blob);
                const link = document.createElement('a');
                link.href = _url
                link.setAttribute('download', name);
                document.body.appendChild(link)
                link.click()
                link.remove()
            })
    }

    useEffect(() => {
        console.log('useEffect')
        getFiles()
    }, []);
    <h1>Мои документы</h1>
    if (files.length === 0) {
        return <h4 className="center">Файлов нет</h4>
    }
    return (
        <div>
            <h1>Мои документы</h1>
            <div className="form-group col-md-12">
                <ul>
                    {files && files.map(file => {
                        return (
                            <li className="file" key={file.url}>
                                <label>
                                    <span onClick={(e) => downloadHandler(e, file.url, `${file.name}`)}>
                                        {file.name} | {new Date(file.created).toDateString()}
                                    </span>
                                    <span>
                                        <i className="material-icons blue-text"
                                        onClick={(e) => downloadHandler(e, file.url, `${file.name}`)}>
                                            file_download
                                        </i>
                                        <i className="material-icons red-text"
                                            onClick={() => onRemoveFile(file.url)}>
                                            delete
                                        </i>
                                    </span>
                                </label>

                            </li>
                        );
                    })
                   }
                </ul>
            </div>


        </div>
    );
};
export default FilesForm;
