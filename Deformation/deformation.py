'''
Created on May 6, 2019

@author: gsq
'''
import numpy as np
import argparse, os
from PIL import Image

#The dimension of space
dim = 2
#The order of the monomial drift
r = 1
#The number of monomials
M = 3
#monomials
def g(t):
    return 1, t[0], t[1]
    
# Radial basis function
def sigma(t, ti):
    L2norm2 = np.sum(np.square(t - ti))
    return L2norm2*np.log(np.sqrt(L2norm2) + 1e-10)
#     return np.sqrt(L2norm2)

def initialization(FLAGS):
    landmarks_o = np.asarray(FLAGS.landmarks_ref)
    landmarks_t = np.asarray(FLAGS.landmarks_img)
    
    # Linear equations
    SIG = np.zeros((landmarks_o.shape[0], landmarks_o.shape[0]))
    D = np.zeros((landmarks_o.shape[0], M))
    
    #Assign values for SIG
    for row in range(SIG.shape[0]):
        for column in range(SIG.shape[1]):
            SIG[row, column] = sigma(landmarks_o[row], landmarks_o[column])
    
    #Assign values for D
    for row in range(D.shape[0]):
        for column in range(D.shape[1]):
            D[row, column] = g(landmarks_o[row])[column]
    
    #Calculate the left matrix of the equations
    left_matr = np.zeros((SIG.shape[0] + D.shape[1], SIG.shape[1] + D.shape[1]))
    left_matr[0:SIG.shape[0], 0:SIG.shape[1]] = SIG
    left_matr[0:SIG.shape[0], SIG.shape[1]:SIG.shape[1] + D.shape[1]] = D
    left_matr[SIG.shape[0]:SIG.shape[0] + D.shape[1], 0:SIG.shape[1]] = np.transpose(D)
    
    #Calculate the right matrix of the equations
    U = np.zeros((SIG.shape[0] + D.shape[1], dim))
    U[0:landmarks_t.shape[0], :] = landmarks_t

    #Calculate the solution of the equations
    equ_coff = np.dot(np.linalg.inv(left_matr), U)



    return equ_coff 

#Interpolated solution
def interpolation(dim, t, equ_coff, FLAGS):
    f = []
    landmarks_ref = np.asarray(FLAGS.landmarks_ref)
    shape = landmarks_ref.shape
    #Compute the coorinate of target
    for l in range(dim):
        ft = np.sum(equ_coff[shape[0]:shape[0]+M, l]*g(t))
        for j in range(shape[0]):
            ft += equ_coff[j, l]*sigma(t, landmarks_ref[j])
        ft = int(ft)
        f.append(ft)


    return f

def main(FLAGS):
    input_img = Image.open(FLAGS.input_img_dir)
    input_ref = Image.open(FLAGS.input_ref_dir)
    img_shape = np.asarray(np.asarray(input_img.size)*FLAGS.fac_def, dtype=np.int)
    ref_shape = np.asarray(np.asarray(input_ref.size)*FLAGS.fac_ref, dtype=np.int)
    input_img = np.asarray(input_img.resize(img_shape))
    input_ref = np.asarray(input_ref.resize(ref_shape))
    img_shape = input_img.shape
    ref_shape = input_ref.shape
    target_img = np.zeros(ref_shape, np.uint8)
    equ_coff = initialization(FLAGS)
    #Obtain the pixel value of target
    
    for row in range(ref_shape[0]):
        for column in range(ref_shape[1]):
            t = [row, column]
            f = interpolation(dim, t, equ_coff, FLAGS)
            if f[0] in range(img_shape[0]) and f[1] in range(img_shape[1]):
                target_img[t[0], t[1], :] = input_img[f[0], f[1], :]
                print(f)

    target_img = Image.fromarray(target_img)
    if os.path.exists('target_img.png'):
        os.remove('target_img.png')
    target_img.save('target_img.png')
    return np.asarray(target_img, np.uint8)
               

if __name__ == '__main__':
#baby
    landmarks_img = '''[[207,128], [211,198], [214, 300], [211, 375], [276, 196], [174, 247],
                     [283, 293], [358, 181], [357, 203], [354, 239], [360, 281], [365, 300]]'''
    landmarks_ref = '''[[30, 131], [51, 216], [45, 288], [15, 365], [351, 149], [20, 248],
                     [356, 328], [368, 115], [403, 153], [439, 228], [416, 325], [368, 388]] '''
    parser = argparse.ArgumentParser()
    parser.add_argument('--input_img_dir', 
                        type=str, 
                        default = 'baby.png',
                        help='The dir of input image'
                        ) 
    parser.add_argument('--input_ref_dir', 
                        type=str, 
                        default = 'ape.png',
                        help='The dir of input reference'
                        )
    parser.add_argument('--landmarks_img',
                        type=str,
                        default = landmarks_img, 
                        help='The landmarks of input image'
                        )
    parser.add_argument('--landmarks_ref', 
                        type=str, 
                        default = landmarks_ref,
                        help='The landmarks of input reference'
                        )
    parser.add_argument('--fac_def', 
                        type=float, 
                        default = 1.0,
                        help='The factor of input image'
                        )
    parser.add_argument('--fac_ref', 
                        type=float, 
                        default = 1.0,
                        help='The factor of input reference'
                        )
    FLAGS, _ = parser.parse_known_args()
    FLAGS.landmarks_img = eval(FLAGS.landmarks_img)
    FLAGS.landmarks_ref = eval(FLAGS.landmarks_ref)
    FLAGS.landmarks_img = np.asarray(FLAGS.landmarks_img, np.int)
    FLAGS.landmarks_ref = np.asarray(FLAGS.landmarks_ref, np.int)
    main(FLAGS)      
