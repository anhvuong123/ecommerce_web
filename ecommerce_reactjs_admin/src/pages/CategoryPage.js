import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Button, Table, Form, Modal } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

const axiosInstance = axios.create({
  baseURL: 'http://localhost:5203/api/',
});

const CategoryPage = () => {
  const [categories, setCategories] = useState([]);
  const [isEditing, setIsEditing] = useState(null);
  const [editedCategory, setEditedCategory] = useState({ categoryId: '', categoryName: '', categoryDescription: '' });
  const [showModal, setShowModal] = useState(false); // Modal state for Create Category
  const navigate = useNavigate();

  const token = localStorage.getItem('token');
  const roles = JSON.parse(localStorage.getItem('roles'));
  
  useEffect(() => {
    if (!token || !roles || !roles.includes('Admin User')) {
      navigate('/login');
    } else {
      axios.get('http://localhost:5203/api/category', {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      })
      .then(response => {
        setCategories(response.data);
      })
      .catch(error => {
        console.error("There was an error fetching the categories!", error);
      });
    }
  }, [navigate]);

  // Custom function to add token for POST/PUT requests
  axiosInstance.interceptors.request.use(
    (config) => {
      if (config.method === 'post' || config.method === 'put') {

        if (token) {
          config.headers['Authorization'] = `Bearer ${token}`;
        }
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  const handleEditClick = (category) => {
    setIsEditing(category.categoryId);
    setEditedCategory(category);
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setEditedCategory((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSaveClick = () => {
    axiosInstance.put(`Category/${editedCategory.categoryId}`, editedCategory)
      .then(response => {
        setCategories(categories.map((category) =>
          category.categoryId === editedCategory.categoryId ? editedCategory : category
        ));
        setIsEditing(null);
      })
      .catch(error => {
        console.error("There was an error updating the category!", error);
      });
  };

  const handleCancelClick = () => {
    setIsEditing(null);
    setEditedCategory({ categoryId: '', categoryName: '', categoryDescription: '' });
  };

  const handleCreateCategory = () => {
    setEditedCategory({ categoryName: '', categoryDescription: '' });
    setIsEditing('new');
    setShowModal(true); // Shhow modal when creating new category
  };

  const handleCreateSaveClick = () => {
    axiosInstance.post('Category', editedCategory)
      .then(response => {
        setCategories([...categories, response.data]);
        setShowModal(false); // Close modal after save
        setEditedCategory({ categoryId: '', categoryName: '', categoryDescription: '' });
      })
      .catch(error => {
        console.error("There was an error creating the category!", error);
      });
  };

  return (
    <div>
      <h2>Manage Categories</h2>
      <Button variant="primary" onClick={handleCreateCategory}>Create Category</Button>
      <Table striped bordered hover className="mt-4">
        <thead>
          <tr>
            <th>Category ID</th>
            <th>Category Name</th>
            <th>Description</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {categories.map((category) => (
            <tr key={category.categoryId}>
              <td>{category.categoryId}</td>
              <td>
                {isEditing === category.categoryId ? (
                  <Form.Control
                    type="text"
                    name="categoryName"
                    value={editedCategory.categoryName}
                    onChange={handleInputChange}
                  />
                ) : (
                  category.categoryName
                )}
              </td>
              <td>
                {isEditing === category.categoryId ? (
                  <Form.Control
                    type="text"
                    name="categoryDescription"
                    value={editedCategory.categoryDescription}
                    onChange={handleInputChange}
                  />
                ) : (
                  category.categoryDescription
                )}
              </td>
              <td>
                {isEditing === category.categoryId ? (
                  <>
                  <div style={{ display: 'flex', gap: '2px' }}>
                    <Button variant="primary" style={{ flex: 1 }} onClick={handleSaveClick}>Save</Button>
                    <Button variant="secondary" style={{ flex: 1 }} onClick={handleCancelClick}>Cancel</Button>
                  </div>
                  </>
                ) : (
                  <Button variant="warning" onClick={() => handleEditClick(category)}>Edit</Button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      {/* Modal for Create Category */}
      <Modal show={showModal} onHide={() => setShowModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Create New Category</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group controlId="categoryName">
              <Form.Label>Category Name <span style={{ color: 'red' }}>*</span></Form.Label> {/* Dấu hoa thị màu đỏ sau Label */}
              <Form.Control
                type="text"
                name="categoryName"
                value={editedCategory.categoryName}
                onChange={handleInputChange}
                required // Đảm bảo trường Category Name là bắt buộc
              />
            </Form.Group>
            <Form.Group controlId="categoryDescription" className="mt-2">
              <Form.Label>Category Description</Form.Label>
              <Form.Control
                type="text"
                name="categoryDescription"
                value={editedCategory.categoryDescription}
                onChange={handleInputChange}
              />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowModal(false)}>Cancel</Button>
          <Button 
            variant="primary" 
            onClick={handleCreateSaveClick}
            disabled={!editedCategory.categoryName} // Disable Save button if Category Name is empty
          >
            Save Category
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};

export default CategoryPage;
