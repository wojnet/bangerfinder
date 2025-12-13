SELECT DB_NAME()

USE FinalAppTemplateDB
GO


-- 1. Users Table
CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY, -- Replaced SERIAL
    name NVARCHAR(50) NOT NULL,            -- Changed to NVARCHAR for better character support
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(), -- Replaced TIMESTAMP with DATETIME

    -- CONSTRAINT
    CONSTRAINT check_email_format CHECK (email LIKE '%@%.%')
);

-- 2. Sessions Table
CREATE TABLE Sessions (
    session_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    token VARCHAR(255) UNIQUE NOT NULL,
    expires_at DATETIME NOT NULL,
    created_at DATETIME DEFAULT GETDATE()
);

-- 3. Songs Table
CREATE TABLE Songs (
    song_id INT IDENTITY(1,1) PRIMARY KEY,
    spotify_id VARCHAR(50) UNIQUE NOT NULL,
    title NVARCHAR(255) NOT NULL,  -- NVARCHAR allows special characters in song titles
    artist NVARCHAR(255) NOT NULL, -- NVARCHAR allows special characters in artist names
    album NVARCHAR(255)
);

-- 4. UserSongFavorites Table
CREATE TABLE UserSongFavorites (
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    song_id INT REFERENCES Songs(song_id) ON DELETE CASCADE,
    added_at DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT pk_user_song_favorite PRIMARY KEY (user_id, song_id)
);

-- 5. Recommendations Table
CREATE TABLE Recommendations (
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    song_id INT REFERENCES Songs(song_id) ON DELETE CASCADE,
    score DECIMAL(5, 2), 
    generated_at DATETIME DEFAULT GETDATE(),
    
    CONSTRAINT pk_recommendation PRIMARY KEY (user_id, song_id)
);