import React from "react"
import {Link, NavLink} from "react-router-dom"

export default class Navbar extends React.Component {
    render()
    {
        return(            
            <nav>
                <h1>Fan App</h1>

                <ul className="nav flex-column">
                    <li className="nav-item px-3">
		            	<NavLink to="/" className="nav-link">
		            		<span className="oi oi-home" aria-hidden="true"></span> Accueil
		            	</NavLink>
		            </li>

                    <li className="nav-item px-3">
		            	<NavLink to="/fanclub" className="nav-link">
		            		<span className="oi oi-plus" aria-hidden="true"></span> Fanclub
		            	</NavLink>
		            </li>
                </ul>
                
            </nav>
        );
    }
}