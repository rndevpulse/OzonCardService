
import { FC, useContext } from 'react';
import * as React from 'react';
import { Link } from 'react-router-dom';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import { Context } from '..';

import './NavMenu.css';
import { observer } from 'mobx-react-lite';

const NavMenu: FC = () => {
    const { loginstore } = useContext(Context);
    console.log('NavMenu store.rules = ', loginstore.rules.toString());

    function linkBasic() {
        if (loginstore.rules.includes('101'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/">Выгрузка</NavLink>
                </NavItem>
                
                )
    };
    function linkReport() {
        if (loginstore.rules.includes('111'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/report">Отчеты</NavLink>
                </NavItem>
            )
    };
    function linkAdmin() {
        if (loginstore.rules.includes('100'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/service">Сервис</NavLink>
                </NavItem>
            )
    };
    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3 grey lighten-3 col s12 m2" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">OzonCardService</NavbarBrand>

                    <ul className="navbar-nav flex-grow">
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
                            <NavLink tag={Link} to="" className="text-dark logout" onClick={() => loginstore.logout()}>Выйти</NavLink>
                        </NavItem>
                    </ul>
                </Container>
            </Navbar>
        </header>
    )
}
export default observer(NavMenu)