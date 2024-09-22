function [map, spectrum] = DiffusionMap(data, outDimensions, numToKeep, rowNormalize)
tic
disp("Making similarity matrix")
% DIFFUSIONMAP Reduces dimension of "data" by optimally respecting diffusion distances 
% after "steps" diffusion steps with transition probabilities determined by
% "kernel". See: Coifman, R. and Stephane Lafon, "Diffusion Maps", Applied
% and Computational Harmonic Analysis, 2006.
% 
% Inputs:
%    data: Array of data points to be mapped. Data points should be stored
%          in the columns of the matrix
%    outDimensions: The number of diffusion dimensions to calculate
%    numToKeep: In the thresholding step, how many nearest neighbors should
%               be kept? Recommended range is 10-30. 
% 
% Outputs:
%    map: The first (outDimensions) rows of the diffusion map. Smallest 
%         eigenvector in first row, etc. 
%    spectrum: The spectrum of corresponding eigenvalues for the map

% ------------------ Input Parsing / Parameter Declaration ---------------
N = size(data, 2);    % number of data points

% -------------Construct the probability transition matrix P--------------
% For iteratively storing the values in a form easy to sparsify
rows = zeros(0,0);
cols = zeros(0,0);
vals = zeros(0,0);
% Sparsifying the relationships
for i = 1:N
    tempKeep = zeros(1,numToKeep);
    for j = 1:N
        if i~=j
            temp = 1/norm(data(:,i)- data(:,j)); % NaN if i==j
            if temp > tempKeep(numToKeep)
                tempKeep(end) = temp;
                tempKeep = sort(tempKeep,'descend');
            end
        end
    end
    threshold = tempKeep(numToKeep);
    for j = 1:N
        if i~=j
            temp = 1/norm(data(:,i)- data(:,j));
            if temp >= threshold
                rows(end+1) = i;
                cols(end+1) = j;
                vals(end+1) = temp;
            end
        end
    end
end
% Resymmetrizing
keps = sparse(rows, cols, vals, N, N);
for i = 1:length(vals)
    if keps(cols(i), rows(i)) == 0
        rows(end+1) = cols(i);
        cols(end+1) = rows(i);
        vals(end+1) = keps(rows(i), cols(i));
    end
end
keps = sparse(rows, cols, vals, N, N);
if (rowNormalize) 
    keps = keps./sum(keps, 2);
    keps = speye(N) - keps;
else
    keps = spdiags(sum(keps, 2), 0, size(keps, 2), size(keps, 2)) - keps;
end

toc
disp("Calculating eigenvectors")
tic
[V, D] = eigs(keps, outDimensions, eps); % want evals near 0

%lam1 = D(2,2); % ignoring the error metric for now
steps = 0; % ignoring the whole scale vectors by their steps thing for now

map = V(:, 2:end);
map = map';
map = D(2:end, 2:end)^steps * map;

spectrum = diag(D);
spectrum = spectrum(2:end);
toc
end
