import React from 'react';
import './App.css';

import { Routes, Route, BrowserRouter } from "react-router-dom";
import Home from './pages/Home';
import FanClub from './pages/FanClub';
import Navbar from './components/Navbar/Navbar';

export default class App extends React.Component<{}, {}> {
  render()
  {
    return(
      <BrowserRouter>
        <Routes>
            <Route path='/' element={<Home />} />
            <Route path='/fanclub' element={<FanClub />} />
          </Routes>
      </BrowserRouter>
    );
  }
}