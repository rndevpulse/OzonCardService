import { FC, useContext, useState } from 'react';
import * as React from 'react'
import { useEffect } from 'react';
import FileService from '../services/FileServise';
import { IFileResponse } from '../models/IFileResponse';

const FilesForm: FC = () => {
    const [files, setFiles] = useState<IFileResponse[]>([])

    async function getFiles() {
        const response = await FileService.getMyFiles()
        setFiles(response.data);
    }

    useEffect(() => {
        getFiles()
    }, []);

    const print = () => {
        
            
        
    }

    return (
        <div>
            <h1>FilesForm</h1>
            <div className="form-group col-md-12">
                <ul>
                    {files && files.map((file, index) => {
                        const classes = ['file']
                        return (
                            <li className={classes.join(' ')} key={index}>
                                <label>
                                    <span>{file.name} | {new Date(file.created).toDateString()}</span>
                                    <label className="details">{file.url}</label>
                                    <i className="material-icons red-text">delete</i>
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
