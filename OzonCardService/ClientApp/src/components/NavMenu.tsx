
import { FC, useContext } from 'react';
import * as React from 'react';
import { Link } from 'react-router-dom';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import { Context } from '..';

import './NavMenu.css';

export const NavMenu: FC = () => {
    const { loginstore } = useContext(Context);
    console.log('NavMenu store.rules = ', loginstore.rules.toString());

    function linkBasic() {
        if (loginstore.rules.includes('101'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/">UploadForm</NavLink>
                </NavItem>
                
                )
    };
    function linkReport() {
        if (loginstore.rules.includes('111'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/report">ReportForm</NavLink>
                </NavItem>
            )
    };
    function linkAdmin() {
        if (loginstore.rules.includes('100'))
            return (
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/service">ServiceForm</NavLink>
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
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/file">FilesForm</NavLink>
                        </NavItem>
                        {linkReport()}
                        {linkAdmin()}
                        <NavItem>
                            <NavLink tag={Link} to="" className="text-dark" onClick={() => loginstore.logout()}>Logout</NavLink>
                        </NavItem>
                    </ul>
                </Container>
            </Navbar>
        </header>
    )
}
