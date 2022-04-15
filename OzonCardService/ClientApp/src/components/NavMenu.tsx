import * as React from 'react'
import { FC } from 'react';
import { Link } from 'react-router-dom';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';

import './NavMenu.css';

 export const NavMenu : FC=()=>(
    <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
            <Container>
                <NavbarBrand tag={Link} to="/">OzonCardService</NavbarBrand>
                        
                    <ul className="navbar-nav flex-grow">
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/">UploadForm</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/auth">LoginForm</NavLink>
                     </NavItem>
                     <NavItem>
                         <NavLink tag={Link} className="text-dark" to="/files">FilesForm</NavLink>
                     </NavItem>
                    </ul>
            </Container>
        </Navbar>
    </header>
)
