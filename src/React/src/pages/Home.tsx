import React from "react"
import Navbar from "../components/Navbar/Navbar";

export default class Home extends React.Component {
    render()
    {
        return(
            <div className="page">
                <div className="sidebar">
                    <Navbar />
                </div>

                <div>Coucou de la home page</div>
            </div>
        );
    }
}