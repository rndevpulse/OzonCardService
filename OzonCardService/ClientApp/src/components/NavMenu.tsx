
import { FC, useContext } from 'react';
import * as React from 'react';
import { Link } from 'react-router-dom';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import { Context } from '..';

import './NavMenu.css';

export const NavMenu: FC = () => {
    const { store } = useContext(Context);
    console.log('store.rules = ', store.rules.toString());

    function linkBasic() {
        if (store.rules.includes('101'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/">UploadForm</NavLink>
                </NavItem>
                
                )
    };
    function linkReport() {
        if (store.rules.includes('111'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/report">ReportForm</NavLink>
                </NavItem>
            )
    };
    function linkAdmin() {
        if (store.rules.includes('100'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/service">ServiceForm</NavLink>
                </NavItem>
            )
    };
    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">OzonCardService</NavbarBrand>

                    <ul className="navbar-nav flex-grow">
                        {linkBasic()}
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/file">FilesForm</NavLink>
                        </NavItem>
                        {linkReport()}
                        {linkAdmin()}
                        <NavItem>
                            <NavLink tag={Link}  to="" className="text-dark" onClick={() => store.logout()}>Logout</NavLink>
                        </NavItem>
                    </ul>
                </Container>
            </Navbar>
        </header>
    )
}
