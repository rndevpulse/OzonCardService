
import { FC, RefObject, useContext, useEffect, useRef, useState } from 'react';
import * as React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import { Context } from '..';

import { observer } from 'mobx-react-lite';
import '../css/NavMenu.css'

const useOnClickOutside = (
    ref: RefObject<HTMLDivElement>,
    closeMenu: () => void
) => {
    useEffect(() => {
        const listener = (event: MouseEvent) => {
            if (ref.current && event.target &&
                ref.current.contains(event.target as Node)
            ) {
                return;
            }
            closeMenu();
        };

        document.addEventListener("mousedown", listener);
        return () => {
            document.removeEventListener("mousedown", listener);
        };
    }, [ref, closeMenu]);
};

const NavMenu: FC = () => {
    const { loginstore } = useContext(Context);
    const navigate = useNavigate()


    const [open, setOpen] = useState<boolean>(false);
    const close = () => setOpen(false);

    const node = useRef<HTMLDivElement>(null);
    useOnClickOutside(node, () => setOpen(false));


    const links: { link: string, name: string }[] = []

    const StyledMenu = ['styledMenu']
    const Menu = ['material-icons']
    if (open) {
        StyledMenu.push('open')
        Menu.push('black-text ')
    }
    else {
        Menu.push('red-text ')
    }

    function linkBasic() {
        if (loginstore.rules.includes('101')) {
            links.push({ link: '/', name: 'Выгрузка' })
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/">Выгрузка</NavLink>
                </NavItem>
                
                )
        }
    };
    function linkReport() {
        if (loginstore.rules.includes('111')) {
            links.push({ link: '/report', name: 'Отчеты' })
            links.push({ link: '/file', name: 'Файлы' })
            links.push({ link: '/task', name: 'Задачи' })
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/report">Отчеты</NavLink>
                </NavItem>
            )
        }
        links.push({ link: '/file', name: 'Файлы' })
        links.push({ link: '/task', name: 'Задачи' })
    };
    function linkAdmin() {
        if (loginstore.rules.includes('100')) {
            links.push({ link: '/service', name: 'Сервис' })
            links.push({ link: '/logout', name: 'Выход' })
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/service">Сервис</NavLink>
                </NavItem>
            )
        }
        links.push({ link: '/logout', name: 'Выход' })
    };

    function onClickLink(l: { link: string, name: string }) {
        close()
        if (l.link === '/logout')
            loginstore.logout()
        else
            navigate(l.link)
    }
    const getLinks = () => {
        return (
            <div className={StyledMenu.join(' ')} ref={node}>
                {links && links.map(l => {
                    return (
                        <div className="styledLink " key={l.name} onClick={(e) => onClickLink(l)}>{l.name}</div>
                    )
                })}
            </div>
        )
    }


    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3 grey lighten-3 col s12 m2" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">OzonCardService</NavbarBrand>

                    <ul className="navbar-nav__custom navbar-nav flex-grow">
                        {linkBasic()}
                        {linkReport()}
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/file">Файлы</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/task">Задачи</NavLink>
                        </NavItem>
                        {linkAdmin()}
                        <NavItem>
                            <NavLink tag={Link} to="" className="text-dark logout" onClick={() => loginstore.logout()}>Выход</NavLink>
                        </NavItem>
                        
                    </ul>
                    <div className="text-dark navbar-nav__button">
                        <i className={Menu.join(' ')}
                            onClick={() => setOpen(!open)}>
                            menu
                        </i>
                        {getLinks()}
                        
                        
                    </div>
                    
                </Container>
            </Navbar>
        </header>
    )
}
export default observer(NavMenu)