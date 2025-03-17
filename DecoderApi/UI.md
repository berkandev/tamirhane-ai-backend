# 📱 Decoder API'nin UI Tarafından Kullanım Kılavuzu

Bu kılavuz, Decoder API'nin kullanıcı arayüzü (UI) tarafından nasıl entegre edileceğini ve kullanılacağını açıklamaktadır.

## 📋 İçindekiler

1. [Genel Bilgiler](#genel-bilgiler)
2. [Kimlik Doğrulama İşlemleri](#kimlik-doğrulama-işlemleri)
3. [Kullanıcı İşlemleri](#kullanıcı-işlemleri)
4. [Araç İşlemleri](#araç-işlemleri)
5. [ECU İşlemleri](#ecu-işlemleri)
6. [Araç-ECU İlişkileri](#araç-ecu-işlemleri)
7. [Yazılım Katalogları](#yazılım-katalogları)
8. [HEX Modifikasyonları](#hex-modifikasyonları)
9. [Hata Yönetimi](#hata-yönetimi)
10. [Örnek UI Akışları](#örnek-ui-akışları)

## 🌐 Genel Bilgiler

### API Base URL
```
http://localhost:5024/api
```

### İstek Formatı
- Tüm istekler JSON formatında gönderilmelidir
- `Content-Type: application/json` header'ı kullanılmalıdır
- Kimlik doğrulama gerektiren isteklerde `Authorization: Bearer {token}` header'ı eklenmelidir

### Örnek Fetch API Kullanımı
```javascript
const apiCall = async (endpoint, method, data, token) => {
  const headers = {
    'Content-Type': 'application/json'
  };
  
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  
  const options = {
    method,
    headers,
    body: method !== 'GET' ? JSON.stringify(data) : undefined
  };
  
  try {
    const response = await fetch(`http://localhost:5024/api${endpoint}`, options);
    
    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Bir hata oluştu');
    }
    
    return await response.json();
  } catch (error) {
    console.error('API çağrısı hatası:', error);
    throw error;
  }
};
```

### Örnek Axios Kullanımı
```javascript
import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:5024/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

// İnterceptor ile token ekleme
apiClient.interceptors.request.use(
  config => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  error => Promise.reject(error)
);
```

## 🔑 Kimlik Doğrulama İşlemleri

### Kullanıcı Girişi
Kullanıcı girişi için aşağıdaki istek yapılmalıdır:

```javascript
// POST /api/users/login
const login = async (username, password) => {
  try {
    const data = await apiCall('/users/login', 'POST', { username, password });
    // Token'ı localStorage veya state'e kaydet
    localStorage.setItem('token', data.token);
    localStorage.setItem('user', JSON.stringify(data.user));
    return data;
  } catch (error) {
    // Hata yönetimi
    throw error;
  }
};
```

### JWT Token'ın Saklanması
JWT token'ı localStorage, sessionStorage veya state yönetim kütüphanesi (Redux, Context API vb.) içinde saklayabilirsiniz:

```javascript
// Token kaydetme
const saveToken = (token, user) => {
  localStorage.setItem('token', token);
  localStorage.setItem('user', JSON.stringify(user));
};

// Token alma
const getToken = () => localStorage.getItem('token');

// Token silme (çıkış yaparken)
const removeToken = () => {
  localStorage.removeItem('token');
  localStorage.removeItem('user');
};
```

### Oturum Kontrolü
Kullanıcının oturumunun açık olup olmadığını kontrol etme:

```javascript
const isAuthenticated = () => {
  const token = localStorage.getItem('token');
  return !!token; // Token varsa true, yoksa false döner
};
```

## 👤 Kullanıcı İşlemleri

### Kullanıcı Kaydı
```javascript
const register = async (userData) => {
  try {
    return await apiCall('/users/register', 'POST', userData);
  } catch (error) {
    throw error;
  }
};
```

### Kullanıcı Profili Görüntüleme
```javascript
const getUserProfile = async () => {
  try {
    return await apiCall('/users/profile', 'GET', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

### Kullanıcı Bilgilerini Güncelleme
```javascript
const updateUser = async (userId, userData) => {
  try {
    return await apiCall(`/users/${userId}`, 'PUT', userData, getToken());
  } catch (error) {
    throw error;
  }
};
```

## 🚗 Araç İşlemleri

### Araçları Listeleme
```javascript
const getVehicles = async () => {
  try {
    return await apiCall('/vehicles', 'GET', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

### Araç Detayı Görüntüleme
```javascript
const getVehicleDetails = async (vehicleId) => {
  try {
    return await apiCall(`/vehicles/${vehicleId}`, 'GET', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

### Araç Ekleme
```javascript
const addVehicle = async (vehicleData) => {
  try {
    return await apiCall('/vehicles', 'POST', vehicleData, getToken());
  } catch (error) {
    throw error;
  }
};
```

### Araç Güncelleme
```javascript
const updateVehicle = async (vehicleId, vehicleData) => {
  try {
    return await apiCall(`/vehicles/${vehicleId}`, 'PUT', vehicleData, getToken());
  } catch (error) {
    throw error;
  }
};
```

### Araç Silme
```javascript
const deleteVehicle = async (vehicleId) => {
  try {
    return await apiCall(`/vehicles/${vehicleId}`, 'DELETE', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

## ⚙️ ECU İşlemleri

### ECU Modellerini Listeleme
```javascript
const getEcuModels = async () => {
  try {
    return await apiCall('/ecu_models', 'GET', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

### ECU Modeli Detayı
```javascript
const getEcuModelDetails = async (ecuModelId) => {
  try {
    return await apiCall(`/ecu_models/${ecuModelId}`, 'GET', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

### ECU Modeli Ekleme
```javascript
const addEcuModel = async (ecuModelData) => {
  try {
    return await apiCall('/ecu_models', 'POST', ecuModelData, getToken());
  } catch (error) {
    throw error;
  }
};
```

## 🔄 Araç-ECU İlişkileri

### Araç-ECU İlişkisi Ekleme
```javascript
const addVehicleEcuModel = async (vehicleId, ecuModelId) => {
  try {
    return await apiCall('/vehicle_ecu_models', 'POST', {
      vehicleId,
      ecuModelId
    }, getToken());
  } catch (error) {
    throw error;
  }
};
```

### Araç-ECU İlişkilerini Listeleme
```javascript
const getVehicleEcuModels = async () => {
  try {
    return await apiCall('/vehicle_ecu_models', 'GET', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

## 📂 Yazılım Katalogları

### Yazılım Kataloglarını Listeleme
```javascript
const getTuneCatalogs = async () => {
  try {
    return await apiCall('/tune_catalogs', 'GET', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

### Yazılım Kataloğu Ekleme
```javascript
const addTuneCatalog = async (tuneCatalogData) => {
  try {
    return await apiCall('/tune_catalogs', 'POST', tuneCatalogData, getToken());
  } catch (error) {
    throw error;
  }
};
```

## 🔧 HEX Modifikasyonları

### HEX Modifikasyonlarını Listeleme
```javascript
const getDiffModifications = async () => {
  try {
    return await apiCall('/diff_modifications', 'GET', null, getToken());
  } catch (error) {
    throw error;
  }
};
```

### HEX Modifikasyonu Ekleme
```javascript
const addDiffModification = async (diffModificationData) => {
  try {
    return await apiCall('/diff_modifications', 'POST', diffModificationData, getToken());
  } catch (error) {
    throw error;
  }
};
```

## ⚠️ Hata Yönetimi

### Hata İşleme Örneği
```javascript
const apiCallWithErrorHandling = async () => {
  try {
    const result = await apiCall('/some-endpoint', 'GET', null, getToken());
    return result;
  } catch (error) {
    if (error.message.includes('401')) {
      // Oturum hatası - kullanıcıyı login sayfasına yönlendir
      redirectToLogin();
    } else if (error.message.includes('403')) {
      // Yetki hatası
      showErrorMessage('Bu işlem için yetkiniz bulunmamaktadır.');
    } else if (error.message.includes('404')) {
      // Kaynak bulunamadı
      showErrorMessage('İstenilen kaynak bulunamadı.');
    } else {
      // Genel hata
      showErrorMessage('Bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
    }
    throw error;
  }
};
```

### Global Hata İşleyici
React uygulamasında global hata yakalama örneği:

```javascript
// ErrorBoundary.jsx
import React from 'react';

class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    console.error('Uygulama hatası:', error, errorInfo);
    // Hata loglama servisi çağrılabilir
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className="error-page">
          <h1>Bir şeyler yanlış gitti</h1>
          <p>Hata: {this.state.error.message}</p>
          <button onClick={() => window.location.reload()}>Sayfayı Yenile</button>
        </div>
      );
    }

    return this.props.children;
  }
}

export default ErrorBoundary;
```

## 📱 Örnek UI Akışları

### Giriş ve Araç Listeleme Akışı

```jsx
// LoginPage.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const loginData = await apiCall('/users/login', 'POST', { username, password });
      localStorage.setItem('token', loginData.token);
      localStorage.setItem('user', JSON.stringify(loginData.user));
      navigate('/dashboard');
    } catch (error) {
      setError('Kullanıcı adı veya şifre hatalı');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <h1>Decoder API'ye Giriş</h1>
      {error && <div className="error-message">{error}</div>}
      <form onSubmit={handleLogin}>
        <div className="form-group">
          <label>Kullanıcı Adı</label>
          <input 
            type="text" 
            value={username} 
            onChange={(e) => setUsername(e.target.value)} 
            required 
          />
        </div>
        <div className="form-group">
          <label>Şifre</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required 
          />
        </div>
        <button type="submit" disabled={loading}>
          {loading ? 'Giriş Yapılıyor...' : 'Giriş Yap'}
        </button>
      </form>
    </div>
  );
};

export default LoginPage;
```

### Araç Listeleme Sayfası

```jsx
// VehicleListPage.jsx
import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

const VehicleListPage = () => {
  const [vehicles, setVehicles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchVehicles = async () => {
      try {
        const data = await apiCall('/vehicles', 'GET', null, localStorage.getItem('token'));
        setVehicles(data);
      } catch (error) {
        setError('Araçlar yüklenirken bir hata oluştu');
      } finally {
        setLoading(false);
      }
    };

    fetchVehicles();
  }, []);

  if (loading) return <div>Yükleniyor...</div>;
  if (error) return <div className="error-message">{error}</div>;

  return (
    <div className="vehicles-container">
      <h1>Araçlar</h1>
      <Link to="/vehicles/add" className="add-button">Yeni Araç Ekle</Link>
      
      {vehicles.length === 0 ? (
        <p>Henüz araç bulunmamaktadır.</p>
      ) : (
        <div className="vehicle-list">
          {vehicles.map(vehicle => (
            <div key={vehicle.id} className="vehicle-card">
              <h3>{vehicle.brand} {vehicle.model}</h3>
              <p>Yıl: {vehicle.year}</p>
              <p>Motor Hacmi: {vehicle.engineVolume}</p>
              <div className="vehicle-actions">
                <Link to={`/vehicles/${vehicle.id}`}>Detaylar</Link>
                <Link to={`/vehicles/${vehicle.id}/edit`}>Düzenle</Link>
                <button onClick={() => handleDeleteVehicle(vehicle.id)}>Sil</button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default VehicleListPage;
```

### Araç Ekleme Formu

```jsx
// AddVehicleForm.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const AddVehicleForm = () => {
  const [vehicleData, setVehicleData] = useState({
    brand: '',
    model: '',
    year: new Date().getFullYear(),
    engineVolume: '',
    productionDate: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setVehicleData({
      ...vehicleData,
      [name]: value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      await apiCall('/vehicles', 'POST', vehicleData, localStorage.getItem('token'));
      navigate('/vehicles');
    } catch (error) {
      setError('Araç eklenirken bir hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="form-container">
      <h1>Yeni Araç Ekle</h1>
      {error && <div className="error-message">{error}</div>}
      
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Marka</label>
          <input 
            type="text" 
            name="brand" 
            value={vehicleData.brand} 
            onChange={handleChange} 
            required 
          />
        </div>
        
        <div className="form-group">
          <label>Model</label>
          <input 
            type="text" 
            name="model" 
            value={vehicleData.model} 
            onChange={handleChange} 
            required 
          />
        </div>
        
        <div className="form-group">
          <label>Yıl</label>
          <input 
            type="number" 
            name="year" 
            value={vehicleData.year} 
            onChange={handleChange} 
            required 
          />
        </div>
        
        <div className="form-group">
          <label>Motor Hacmi</label>
          <input 
            type="text" 
            name="engineVolume" 
            value={vehicleData.engineVolume} 
            onChange={handleChange} 
            required 
          />
        </div>
        
        <div className="form-group">
          <label>Üretim Tarihi</label>
          <input 
            type="date" 
            name="productionDate" 
            value={vehicleData.productionDate} 
            onChange={handleChange} 
            required 
          />
        </div>
        
        <div className="form-actions">
          <button type="button" onClick={() => navigate('/vehicles')}>İptal</button>
          <button type="submit" disabled={loading}>
            {loading ? 'Kaydediliyor...' : 'Kaydet'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddVehicleForm;
```

## 🔒 Güvenlik İpuçları

1. **Token güvenliği**:
   - JWT token'ı HTTP-only cookie'lerde saklamak localStorage'dan daha güvenlidir
   - Hassas bilgileri token içinde saklamaktan kaçının

2. **Input validasyonu**:
   - Kullanıcı girdilerini hem client hem de server tarafında doğrulayın
   - XSS ve SQL enjeksiyon saldırılarına karşı girdileri temizleyin

3. **CORS yönetimi**:
   - Cross-Origin Resource Sharing (CORS) ayarlarını doğru yapılandırın
   - Sadece güvendiğiniz domain'lere erişim izni verin

4. **API isteklerinde rate limiting**:
   - Kısa sürede çok fazla istek göndermeyin
   - Başarısız giriş denemelerinden sonra bir süre bekleyin

## 📊 UI Performans İpuçları

1. **Veri önbelleğe alma**:
   - Sık kullanılan verileri önbelleğe alın (React Query, SWR gibi kütüphanelerle)
   - Gereksiz API çağrılarından kaçının

2. **Sayfalama ve filtreleme**:
   - Büyük veri listeleri için sayfalama kullanın
   - Sunucu taraflı filtreleme ve sıralama özelliklerini kullanın

3. **Gecikmeli yükleme (Lazy loading)**:
   - Büyük komponentleri gecikmeli olarak yükleyin
   - Resimleri ve ağır medya dosyalarını gecikmeli yükleyin

4. **State yönetimi**:
   - Karmaşık UI'lar için Redux veya Context API kullanın
   - Bileşen state'lerini mümkün olduğunca yerel tutun 