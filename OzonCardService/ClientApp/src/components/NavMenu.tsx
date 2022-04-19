
import { FC, useContext } from 'react';
import { Link } from 'react-router-dom';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import { Context } from '..';

import './NavMenu.css';

export const NavMenu: FC = () => {
    const { store } = useContext(Context);
    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">OzonCardService</NavbarBrand>

                    <ul className="navbar-nav flex-grow">
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/">UploadForm</NavLink>
                        </NavItem>

                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/files">FilesForm</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link}  to="" className="text-dark" onClick={() => store.logout()}>Logout</NavLink>
                        </NavItem>
                    </ul>
                </Container>
            </Navbar>
        </header>
    )
}
