
import { FC, RefObject, useContext, useEffect, useRef, useState } from 'react';
import * as React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import { Context } from '../../';
import { observer } from 'mobx-react-lite';
import './index.css'


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
    const { loginStore } = useContext(Context);
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
        if (loginStore.Roles.includes('Basic')) {
            links.push({ link: '/', name: 'Выгрузка' })
            links.push({ link: '/search', name: 'Поиск' })
            return (
                <div>
                    <NavItem>
                        <NavLink tag={Link} className="text-dark" to="/">Выгрузка</NavLink>
                    </NavItem>
                    <NavItem>
                        <NavLink tag={Link} className="text-dark" to="/search">Поиск</NavLink>
                    </NavItem>
                </div>
            )
        }
    }
    function linkReport() {
        if (loginStore.Roles.includes('Report')) {
            links.push({ link: '/report', name: 'Отчеты' })
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/report">Отчеты</NavLink>
                </NavItem>
            )
        }
    }
    function linkAdmin() {
        if (loginStore.Roles.includes('Admin')) {
            links.push({ link: '/service', name: 'Сервис' })
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/service">Сервис</NavLink>
                </NavItem>
            )
        }
    }

    function onClickLink(l: { link: string, name: string }) {
        close()
        if (l.link === '/logout')
            loginStore.logout()
        else
            navigate(l.link)
    }
    const getLinks = () => {
        links.push({ link: '/files', name: 'Файлы' })
        links.push({ link: '/tasks', name: 'Задачи' })
        links.push({ link: '/logout', name: 'Выход' })
        return (
            <div className={StyledMenu.join(' ')} ref={node}>
                {links && links.map(l => {
                    return (
                        <div className="styledLink " key={l.name} onClick={() => onClickLink(l)}>{l.name}</div>
                    )
                })}
            </div>
        )
    }


    return (
        <header>
            <Navbar className="navbar-expand navbar-toggleable-sm border-bottom box-shadow grey mb-3  lighten-3 col s12 m2" light>
                <Container className="container-nav">

                    <div >
                        <img src="../logo.png" alt="logo" className="logo_image"/>
                        <NavbarBrand tag={Link} to="/" >Corporate Catering Card Service</NavbarBrand>
                    </div>


                    <ul className="navbar-nav__custom navbar-nav flex-grow">
                        {linkBasic()}
                        {linkReport()}
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/files">Файлы</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/tasks">Задачи</NavLink>
                        </NavItem>
                        {linkAdmin()}
                        <NavItem>
                            <NavLink tag={Link} to="" className="text-dark logout" onClick={() => loginStore.logout()}>Выход</NavLink>
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