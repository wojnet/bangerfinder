-- 1. Users Table (Updated)
CREATE TABLE Users (
    user_id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    
    -- Coloane noi adaugate:
    is_verified BOOLEAN NOT NULL DEFAULT FALSE, -- Corespunde cu bool in C#
    verification_token VARCHAR(100),            -- Poate fi NULL
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    -- SIMPLE, READABLE CONSTRAINT (Oracle Style)
    CONSTRAINT check_email_format CHECK (email LIKE '%@%.%')
);

-- 2. Sessions Table
CREATE TABLE Sessions (
    session_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    token VARCHAR(255) UNIQUE NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 3. Songs Table
CREATE TABLE Songs (
    song_id SERIAL PRIMARY KEY,
    spotify_id VARCHAR(50) UNIQUE NOT NULL,
    title VARCHAR(255) NOT NULL,
    artist VARCHAR(255) NOT NULL,
    album VARCHAR(255)
);

-- 4. UserSongFavorites Table
CREATE TABLE UserSongFavorites (
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    song_id INT REFERENCES Songs(song_id) ON DELETE CASCADE,
    added_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_user_song_favorite PRIMARY KEY (user_id, song_id)
);

-- 5. Recommendations Table
CREATE TABLE Recommendations (
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    song_id INT REFERENCES Songs(song_id) ON DELETE CASCADE,
    score DECIMAL(5, 2), 
    generated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_recommendation PRIMARY KEY (user_id, song_id)
);