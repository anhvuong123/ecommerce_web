import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Button, Table, Pagination, Image, Form , Modal, Alert} from 'react-bootstrap';
import { useNavigate } from 'react-router-dom'; // Import useNavigate

const axiosInstance = axios.create({
  baseURL: 'http://localhost:5203/api/',
});

const ProductPage = () => {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(10);
  const [isEditing, setIsEditing] = useState(null);
  const [selectedImage, setSelectedImage] = useState(null);
  const [uploadedImage, setUploadedImage] = useState(null);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [newProduct, setNewProduct] = useState({
    productName: '',
    categoryId: '',
    productDescription: '',
    productPrice: null,
    productImageUrl: '',
  }); // to manage new product data
  const [priceValid, setPriceValid] = useState(true); 
  const navigate = useNavigate(); 
  const [showSuccessPopup, setShowSuccessPopup] = useState(false);


  const token = localStorage.getItem('token');
  const roles = JSON.parse(localStorage.getItem('roles'));

  useEffect(() => {
    if (!token || !roles || !roles.includes('Admin User')) {
      navigate('/login');
    } else {
      axios.get('http://localhost:5203/api/product', {
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
  
  // Fetch Data
  const fetchCategories = async () => {
    try {
      const response = await axiosInstance.get('Category');
      setCategories(response.data);
    } catch (error) {
      console.error("Error while fetching categories !", error);
    }
  };

  const fetchProducts = async (pageNumber = 1, pageSize = 10) => {
    try {
      const response = await axiosInstance.get('Product/pagination', {
        params: { pageNumber, pageSize },
      });

      if (response.data && response.data.items && Array.isArray(response.data.items)) {
        setProducts(response.data.items);
        setTotalPages(response.data.totalPages);
      } else {
        setProducts([]);
      }
    } catch (error) {
      console.error("Error while fetching categories !", error);
      setProducts([]);
    }
  };

  useEffect(() => {
    fetchCategories();
    fetchProducts(currentPage, pageSize);
  }, [currentPage, pageSize]);

  // Handlers
  const handleEditClick = (productId) => setIsEditing(productId);

  const handleInputChange = (e, productId) => {
    const { name, value } = e.target;
    setProducts((prev) =>
      prev.map((product) =>
        product.productId === productId ? { ...product, [name]: value } : product
      )
    );
  };

  const handleInputChangePopup = (e) => {
    const { name, value } = e.target;
    setNewProduct((prev) => ({
      ...prev,
      [name]: value, 
    }));
  };
   

  const handleCategoryChange = (e, productId) => {
    const { value } = e.target;
    setProducts((prev) =>
      prev.map((product) =>
        product.productId === productId ? { ...product, category: { categoryId: value } } : product
      )
    );
  };

  const handleImageChange = (e, productId) => {
      const file = e.target.files[0];
      if (file) {
        const imagePreview = URL.createObjectURL(file);
        setUploadedImage(imagePreview); // Hiển thị ảnh tạm thời trên UI

        // Lưu tên tệp ảnh
        setProducts((prev) =>
          prev.map((product) =>
            product.productId === productId
              ? { ...product, tempImageFile: file, imageName: file.name } // Lưu tên file ảnh
              : product
          )
        );
      }
    };

    const handleImageCreate = (e) => {
      const file = e.target.files[0];
      if (file) {
        // hiển thị ảnh tạm thời trên UI
        const imagePreview = URL.createObjectURL(file);
        setUploadedImage(imagePreview); 

        setNewProduct((prevProduct) => ({
          ...prevProduct,
          productImageUrl: file.name,
        }));
      }
    };

  const handleSaveClick = async (productId) => {
    const product = products.find((p) => p.productId === productId);
    console.log("!!!Temp image file:", product.tempImageFile);
    console.log("Updated products after image change:", products);
  
    if (product.tempImageFile) {
      const formData = new FormData();
      formData.append("image", product.tempImageFile);
    
      try {
        const response = await axiosInstance.post("Product/upload", formData, {
          headers: { "Content-Type": "multipart/form-data" },
        });
        
        product.productImageUrl = `/uploads/${product.imageName}`;
      } catch (error) {
        console.error("Có lỗi khi upload hình ảnh!", error);
        return;
      }
    }
  
    const updatedProduct = {
      ...product,
      categoryId: product.category?.categoryId || product.categoryId,
      productImageUrl: product.productImageUrl,
    };
  
    try {
      await axiosInstance.put(`Product/${productId}`, updatedProduct);
      fetchProducts(currentPage, pageSize);
      setIsEditing(null);
      setSelectedImage(null);
      setUploadedImage(null); // Reset preview
    } catch (error) {
      console.error("Có lỗi khi cập nhật sản phẩm!", error);
    }
  };
  
  const handleCancelClick = () => {
      setIsEditing(null);
      setSelectedImage(null);
      setUploadedImage(null); // Reset preview
  };

  const handleCreateProduct = async () => {
  if (!newProduct.productName || !newProduct.categoryId) {
    console.log("Please fill out all required fields.");
    return; // Dừng nếu thông tin không đầy đủ
  }

  if (newProduct.productImageUrl) {
    const formData = new FormData();
    formData.append("image", newProduct.productImageUrl);
    
    try {
      // Upload hình ảnh lên backend
      const response = await axiosInstance.post("Product/upload", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      const imageUrl = `/uploads/${newProduct.productImageUrl}`;
      const newProductData = {
        ...newProduct,
        productImageUrl: imageUrl, 
        productImage: null,
      };

      // Gửi yêu cầu tạo sản phẩm với thông tin sản phẩm đã hoàn chỉnh
      await axiosInstance.post("Product", newProductData);
      console.log("Product created successfully!");

      // Reset form và đóng modal sau khi tạo sản phẩm thành công
      setNewProduct({
        productName: '',
        categoryId: '',
        productDescription: '',
        productPrice: null,
        productImage: '', //Clear the product image
      });
      setShowCreateModal(false);  //Đóng modal
      setShowSuccessPopup(true);  //Mở popup success
      //Đặt timeout để tự động ẩn thông báo sau 5 giây
      setTimeout(() => {
        setShowSuccessPopup(false);
      }, 5000);  // 5000ms = 5 giây

    } catch (error) {
      console.error("Có lỗi khi upload hình ảnh hoặc tạo sản phẩm mới!", error);
    }
  } else {
    // If no image, just create the product with other details
    try {
      const newProductData = {
        ...newProduct,
        productImageUrl: null, // No image
      };

      await axiosInstance.post("Product", newProductData);
      console.log("Product created successfully!");

      // Reset form after creation
      setNewProduct({
        productName: '',
        categoryId: '',
        productDescription: '',
        productPrice: 0,
        productImage: null,
      });
      setShowCreateModal(false);  // Close modal
      setShowSuccessPopup(true);  // Show success message

    } catch (error) {
      console.error("Error while create new product !", error);
    }
  }
};

  
  useEffect(() => {
    if (showSuccessPopup) {
      const timer = setTimeout(() => {
        setShowSuccessPopup(false);
      }, 5000);

      // Cleanup function để clearTimeout khi component unmount hoặc khi showSuccessPopup được thay đổi
      return () => clearTimeout(timer);
    }
  }, [showSuccessPopup]);

  // Price validation to ensure it's a valid number (integer or decimal)
  const handlePriceChange = (e) => {
    const value = e.target.value;
    const validPrice = /^[0-9]*\.?[0-9]+$/.test(value); // Regex to check if it's a valid number or decimal

    if (validPrice || value === null) { // Allow empty field
      setNewProduct((prev) => ({
        ...prev,
        productPrice: value,
      }));
      setPriceValid(true);
    } else {
      setPriceValid(false);
    }
  };

   // Handle modal visibility
   const handleShowCreateModal = () => setShowCreateModal(true);
   const handleCloseCreateModal = () => setShowCreateModal(false);

   // Disable "Save Product" button if Product Name or Category is empty
  const isSaveButtonDisabled = !newProduct.productName || !newProduct.categoryId || !priceValid;
  
  // Render UI
  return (
    <div>
      {showSuccessPopup && (
        <Alert variant="success" onClose={() => setShowSuccessPopup(false)} dismissible>
          <Alert.Heading>Product Created Successfully!</Alert.Heading>
          <p>
            Your product has been created successfully. You can now view it in the product list.
          </p>
        </Alert>
      )}
      <h2>Manage Products</h2>
      {token && roles && ( // Only show Create Product button if logged in and is admin
        <Button variant="primary" className="mb-3" onClick={handleShowCreateModal}>
          Create Product
        </Button>
      )}
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Category</th>
            <th>Description</th>
            <th>Price</th>
            <th>Image</th>
            <th>CreatedDate</th>
            <th>UpdatedDate</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {products.length > 0 ? (
            products.map((product) => (
              <tr key={product.productId}>
                <td>{product.productId}</td>
                <td>
                  {isEditing === product.productId ? (
                    <Form.Control
                      type="text"
                      name="productName"
                      value={product.productName}
                      onChange={(e) => handleInputChange(e, product.productId)}
                    />
                  ) : (
                    product.productName
                  )}
                </td>
                <td>
                  {isEditing === product.productId ? (
                    <Form.Control
                      as="select"
                      name="category"
                      value={product.category?.categoryId || ''}
                      onChange={(e) => handleCategoryChange(e, product.productId)}
                    >
                      {categories.map((category) => (
                        <option key={category.categoryId} value={category.categoryId}>
                          {category.categoryName}
                        </option>
                      ))}
                    </Form.Control>
                  ) : (
                    product.category?.categoryName || 'N/A'
                  )}
                </td>
                <td>
                  {isEditing === product.productId ? (
                    <Form.Control
                      type="text"
                      name="productDescription"
                      value={product.productDescription}
                      onChange={(e) => handleInputChange(e, product.productId)}
                    />
                  ) : (
                    product.productDescription
                  )}
                </td>
                <td>
                  {isEditing === product.productId ? (
                    <Form.Control
                      type="number"
                      name="productPrice"
                      value={product.productPrice}
                      onChange={(e) => handleInputChange(e, product.productId)}
                    />
                  ) : (
                    product.productPrice
                  )}
                </td>
                <td className="text-center">
                {isEditing === product.productId ? (
                  <>
                    <input
                      type="file"
                      onChange={(e) => handleImageChange(e, product.productId)}
                      style={{ display: 'none' }}
                      id={`upload_${product.productId}`}
                    />
                    <Button
                      size="sm"
                      onClick={() => document.getElementById(`upload_${product.productId}`).click()}
                    >
                      Browse
                    </Button>
                    {uploadedImage && (
                      <div>
                        <img src={uploadedImage} alt="Preview" width={150} />
                      </div>
                    )}
                  </>
                ) : (
                  product.productImageUrl && (
                    <Image
                      src={`http://localhost:5203${product.productImageUrl}`}
                      alt={product.productName}
                      width={150}
                    />
                  )
                )}
              </td>

              <td>
              {product.createdDate 
                ? new Date(product.createdDate).toLocaleString('sv-SE')
                : ''
              }
            </td>
            <td>
              {product.updatedDate 
                ? new Date(product.updatedDate).toLocaleString('sv-SE')
                : ''
              }
            </td>

                {token && roles && ( // Conditionally render the action buttons
                  <td>
                    {isEditing === product.productId ? (
                      <>
                      <div style={{ display: 'flex', gap: '2px' }}>
                        <Button variant="primary" style={{ flex: 1 }} onClick={() => handleSaveClick(product.productId)}>
                          Save
                        </Button>
                        <Button variant="secondary" style={{ flex: 1 }} onClick={handleCancelClick}>
                          Cancel
                        </Button>
                        </div>
                      </>
                    ) : (
                      <>
                        <Button variant="warning" onClick={() => handleEditClick(product.productId)}>
                          Edit
                        </Button>
                      </>
                    )}
                  </td>
                )}
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="8">No products available.</td>
            </tr>
          )}
        </tbody>
      </Table>
      <Pagination>
        {[...Array(totalPages)].map((_, index) => (
          <Pagination.Item
            key={index}
            active={index + 1 === currentPage}
            onClick={() => setCurrentPage(index + 1)}
          >
            {index + 1}
          </Pagination.Item>
        ))}
      </Pagination>

      {/* Modal for Creating Product */}
      <Modal show={showCreateModal} onHide={handleCloseCreateModal}>
        <Modal.Header closeButton>
          <Modal.Title>Create New Product</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group controlId="productName">
              <Form.Label>
                Product Name <span style={{ color: 'red' }}>*</span>
              </Form.Label>
              <Form.Control
                type="text"
                name="productName"
                value={newProduct.productName}
                onChange={handleInputChangePopup}
                required
              />
            </Form.Group>
            <Form.Group controlId="category">
              <Form.Label>
                Category <span style={{ color: 'red' }}>*</span>
              </Form.Label>
              <Form.Control
                as="select"
                name="categoryId"
                value={newProduct.categoryId}
                onChange={handleInputChangePopup}
                required
              >
                <option value="">Select Category</option>
                {categories.map((category) => (
                  <option key={category.categoryId} value={category.categoryId}>
                    {category.categoryName}
                  </option>
                ))}
              </Form.Control>
            </Form.Group>
            <Form.Group controlId="productDescription">
              <Form.Label>Description</Form.Label>
              <Form.Control
                as="textarea"
                rows={3}
                name="productDescription"
                value={newProduct.productDescription}
                onChange={handleInputChangePopup}
              />
            </Form.Group>
            <Form.Group controlId="productPrice">
              <Form.Label>Price</Form.Label>
              <Form.Control
                type="text"
                name="productPrice"
                value={newProduct.productPrice}
                onChange={handlePriceChange}
                isInvalid={!priceValid}
              />
              {!priceValid && <Form.Control.Feedback type="invalid">Please enter a valid price.</Form.Control.Feedback>}
            </Form.Group>
            {/* <Form.Group controlId="productImage">
              <Form.Label>Image</Form.Label>
              <Form.Control
                type="file"
                onChange={handleImageChange}
                accept="image/*"
              />
              {uploadedImage && <img src={uploadedImage} alt="Preview" width={150} />}
            </Form.Group> */}
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseCreateModal}>
            Close
          </Button>
          <Button variant="primary" onClick={handleCreateProduct} disabled={isSaveButtonDisabled}>
            Save Product
          </Button>
        </Modal.Footer>
      </Modal>
    
    </div>
  );
};

export default ProductPage;