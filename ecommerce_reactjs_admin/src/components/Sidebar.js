// src/components/Sidebar.js

import React from 'react';
import { Link } from 'react-router-dom';
import { Nav } from 'react-bootstrap';

const Sidebar = () => {
  return (
    <div className="sidebar">
      <Nav defaultActiveKey="/home" className="flex-column">
        <Nav.Item>
          <Link to="/manage-category">Manage Category</Link>
        </Nav.Item>
        <Nav.Item>
          <Link to="/manage-product">Manage Product</Link>
        </Nav.Item>
      </Nav>
    </div>
  );
};

export default Sidebar;
